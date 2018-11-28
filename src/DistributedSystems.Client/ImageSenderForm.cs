using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DistributedSystems.Client
{
    public partial class ImageSenderForm : Form
    {
        private readonly ImageHelper _imageHelper;

        public ImageSenderForm()
        {
            InitializeComponent();
            _imageHelper = new ImageHelper();
        }

        private async void SelectImageButton_Click(object sender, EventArgs e)
        {
            var dialogResult = FilePickerDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var file = FilePickerDialog.FileName;

                await SortOutImageStuff(file);
            }


        }

        private async Task SortOutImageStuff(string file)
        {
            await _imageHelper.DoImageStuff(file);
        }
    }
}
