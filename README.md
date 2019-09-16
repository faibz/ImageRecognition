# Image Recognition

This is scaling image recognition system written in C#. Using Azure's Computer Vision API and various other components, this system can analyse very large pictures with a scaling worker pool designed to scale down its processing power when not in use and scale up when under heavy use.

## Components

This system is comprised of various components that work together to process an image.

### DistributedSystems.Client

The client is the front-end application of the system. This application is where the user will select an image to analyse. The selected image will then be broken down into 1000x1000 pieces and sent off to the API. After sending the image to the API, the client will then check to see if any of those 1000x1000 pieces have been processed by asking the API if there are any available results of processing the whole image. Should any results be found and returned by the API, they will be shown to the user in a table.

### DistributedSystems.API

The API is the central application...

## System Design

![System Design](https://github.com/faibz/image-recognition/blob/master/systemdesign.png "System Design")

#### Credit

Thank you to my friend https://github.com/amrwc for his work on the DistributedSystems.Client project.
