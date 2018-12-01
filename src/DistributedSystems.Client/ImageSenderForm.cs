using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using DistributedSystems.Shared.Models;
using Newtonsoft.Json;

namespace DistributedSystems.Client
{
    public partial class ImageSenderForm : Form
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ImageHelper _imageHelper;

        private Guid _mapId = Guid.Empty;
        private BindingSource _tagDataSource;
        private readonly List<Tag> _tagData = new List<Tag>();

        public ImageSenderForm()
        {
            InitializeComponent();
            _httpClient.BaseAddress = new Uri("https://distsysimageapi.azurewebsites.net/api/"); // requests using this must have not have a '/' at the start of path
            _imageHelper = new ImageHelper(_httpClient);

            imageTagsPoller.Interval = 1000;
            imageTagsPoller.Tick += TagsTimer_Tick;
        }

        private async void SelectImageButton_Click(object sender, EventArgs e)
        {
            var dialogResult = FilePickerDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var file = FilePickerDialog.FileName;

                var imageResult = await _imageHelper.SendImageRequest((file));
                if (!imageResult.success) return;

                _mapId = imageResult.mapId;

                ShowTagsInformation();
            }
        }

        private async void TagsTimer_Tick(object sender, EventArgs e)
        {
            var tagResult = await _httpClient.GetAsync($"Tags/GetTagsForMapId/{_mapId}");
            var tagData = JsonConvert.DeserializeObject<List<Tag>>(await tagResult.Content.ReadAsStringAsync());
            ProcessTagData(tagData);
        }

        private void ProcessTagData(ICollection<Tag> tagData)
        {
            if (tagData == null) return;

            foreach (var existingTag in _tagData)
            {
                var matchingTag = tagData.FirstOrDefault(tag => tag.Name == existingTag.Name);
                if (matchingTag != null) tagData.Remove(matchingTag);
            }

            foreach (var tag in tagData)
            {
                _tagData.Add(tag);
                _tagDataSource.Add(tag);
            }

        }

        private void ShowTagsInformation()
        {
            selectImageButton.Hide();
            tagsLabel.Show();
            _tagDataSource = new BindingSource { DataSource = new List<Tag>() };
            tagDataGrid.DataSource = _tagDataSource;
            tagsPanel.Show();
            imageTagsPoller.Start();
        }
    }
}
