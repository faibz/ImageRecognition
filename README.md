# Image Recognition

This is scaling image recognition system written in C#. Using Azure's Computer Vision API and various other components, this system can analyse very large pictures with a scaling worker pool designed to scale down its processing power when not in use and scale up when under heavy use.

## Components

This system is comprised of various components that work together to process an image.

### DistributedSystems.Client

_.NET Framework 4.6.1 WinForms Application_

The client is the front-end application of the system. This application is where the user will select an image to analyse. The selected image will then be broken down into 1000x1000 pieces and sent off to the API. After sending the image to the API, the client will then check to see if any of the image pieces have been processed by asking the API if there are any available results of processing the whole image. Should any results be found and returned by the API, they will be shown to the user in a table.

### DistributedSystems.API

_.NET Core 2.1 Web API Application_

The API is the central application in the system. This application is responsible for most of the work in the system, and therefore, each controller will be described individually:
* Data Controller - Provides data about image processing times. Consumed by the DistributedSystems.WorkerManager project. 
* Images Controller - Provides endpoints for uploading images and retrieving the status of them. Consumed by Client project.
* Maps Controller - Provides a single endpoint for creating an image map (visualisation of a map can be seen below). Consumed by the Client project when breaking down a large image.
* Tags Controller - Provides endpoints for the Worker project to send post-analysis image data and for the Client project to ask for the aforementioned image data. Consumed by Worker and Client projects.
** Note: If the image analysis data contains results (tags) with low confidence, the API will then add a message onto the process request queue with a neighbouring tile for further processing.
* Versions Controller - Provides information about DistributedSystems.Worker versions for updating purposes. Could, in future, be consumed by the Worker project to know when to update.

Underneath the controller layer, there are some services that follow up some of the processing required by the controllers. Three important responsibilities on the service layer are:
* Uploading the image - When image pieces come in, they need to be uploaded to Blob storage so they can be accessed by the Worker project.
* Sending a processing request - For the Worker project to know it needs to process an image, a message gets placed on a service bus queue which the Worker project listens to.
* Storing the image data - To keep track of the image and map data, some data gets stored in a database.

### DistributedSystems.Worker

_.NET Core 2.1 Console Application_

The Worker project is responsible for processing images. It does this in several steps. Firstly, the Worker listens to the process request queue. When it has a message from the queue, it will download the image(s) specified in the message. If necessary, the Worker will then combine the images (with respect to the positions in the map) into a single image. Next, the Worker will send the image to Azure's Computer Vision API. Lastly, the Worker will send the information returned by the Computer Vision API to DistributedSystems.API for storage.

### DistributedSystems.WorkerManager

_.NET Framework 4.7 Windows Service (using TopShelf)_

The WorkerManager process is responsible for scaling the worker pool to adjust to system load. This is a simple process but it was written with expansion in mind. The WorkerManager uses a few metrics to decide what action it should take (add worker, remove worker, do nothing): current messages in the queue, current average processing time per image, and target processing time. It processes these metrics, decides on an action, and then executes it.

### DistributedSystems.Migrations

_.NET Core 2.1 Console Application (using Simple.Migrations)_

The Migrations project is a simple project that simply migrates a database to a specific version (typically latest) of the migrations.

## System Design

![System Design](https://github.com/faibz/image-recognition/blob/master/systemdesign.png "System Design")

## Miscellaneous

### Hosting

This application was built and deployed via Azure DevOps to various components hosted in Azure.

### Map Visualisation

When a large image gets selected by the user on the client. It gets broken down into a map comprised of several tiles. Each of these tiles are firstly processed individually. However, if Azure's Computer Vision API returns image data with low confidence levels, the API will submit an image process request for a compound image. This compound image will contain the original tile and another that is connected to it. This process can be repeated until the data from the Computer Vision API is satisfactory.

![Map Visualisation](https://github.com/faibz/image-recognition/blob/master/mapvisualisation.png "Map Visualisation")

#### Credit

Thank you to my friend https://github.com/amrwc for his work on the DistributedSystems.Worker project.
