# Image Recognition

This is scaling image recognition system written in C#. Using Azure's Computer Vision API and various other components, this system can analyse very large pictures with a scaling worker pool designed to scale down its processing power when not in use and scale up when under heavy use.

## Components

This system is comprised of various components that work together to process an image.

### DistributedSystems.Client

.NET Framework 4.6.1 WinForms Application

The client is the front-end application of the system. This application is where the user will select an image to analyse. The selected image will then be broken down into 1000x1000 pieces and sent off to the API. After sending the image to the API, the client will then check to see if any of the image pieces have been processed by asking the API if there are any available results of processing the whole image. Should any results be found and returned by the API, they will be shown to the user in a table.

### DistributedSystems.API

.NET Core 2.1 Web API Application

The API is the central application in the system. This application is responsible for most of the work in the system, and therefore, each controller will be described individually:
* Data Controller - Provides data about image processing times. Consumed by the DistributedSystems.WorkerManager project. 
* Images Controller - Provides endpoints for uploading images and retrieving the status of them. Consumed by Client project.
* Maps Controller - Provides a single endpoint for creating an image map (visualisation of a map can be seen below). Consumed by the Client project when breaking down a large image.
* Tags Controller - Provides endpoints for the Worker project to send post-analysis image data and for the Client project to ask for the aforementioned image data. Consumed by Worker and Client projects.
* Versions Controller - Provides information about DistributedSystems.Worker versions for updating purposes. Could, in future, be consumed by the Worker project to know when to update.

Underneath the controller layer, there are some services that follow up some of the processing required by the controllers. Three important responsibilities on the service layer are:
* Uploading the image - When image pieces come in, they need to be uploaded to Blob storage so they can be accessed by the Worker project.
Sending a processing require - For the Worker project to know it needs to process an image, a message gets placed on a service bus queue which the Worker project listens to.
* Storing the image data - To keep track of the image and map data, some data gets stored in a database.

### DistributedSystems.Worker

.NET Core 2.1 Console Application

### DistributedSystems.WorkerManager

.NET Framework 4.7 Windows Service (using TopShelf)

### DistributedSystems.Migrations

.NET Core 2.1 Console Application

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
