# Image Recognition

This is scaling image recognition system written in C#. Using Azure's Computer Vision API and various other components, this system can analyse very large pictures with a scaling worker pool designed to scale down its processing power when not in use and scale up when under heavy use.

## Components

This system is comprised of various components that work together to process an image.

### DistributedSystems.Client

The client is the front-end application of the system. This application is where the user will select an image to analyse. The selected image will then be broken down into 1000x1000 pieces and sent off to the API. After sending the image to the API, the client will then check to see if any of the image pieces have been processed by asking the API if there are any available results of processing the whole image. Should any results be found and returned by the API, they will be shown to the user in a table.

### DistributedSystems.API

The API is the central application in the system. This application is responsible for most of the work in the system, and therefore, each controller will be described individually:
* Data Controller - Provides data about image processing times. Consumed by the DistributedSystems.WorkerManager project. 
* Images Controller - Provides endpoints for uploading images and retrieving the status of them. Consumed by Client project.
* Maps Controller - Provides a single endpoint for creating an image map (visualisation of a map can be seen below). Consumed by the Client project when breaking down a large image.
* Tags Controller - Provides endpoints for the Worker project to send post-analysis image data and for the Client project to ask for the aforementioned image data. Consumed by Worker and Client projects.
* Versions Controller - Provides information about DistributedSystems.Worker versions for updating purposes. Could, in future, be consumed by the Worker project to know when to update.

## System Design

![System Design](https://github.com/faibz/image-recognition/blob/master/systemdesign.png "System Design")

## Miscellaneous

### Hosting

This application was built and deployed via Azure DevOps to various components hosted in Azure.

### Map Visualisation

![Map Visualisation](https://github.com/faibz/image-recognition/blob/master/mapvisualisation.png "Map Visualisation")

#### Credit

Thank you to my friend https://github.com/amrwc for his work on the DistributedSystems.Client project.
