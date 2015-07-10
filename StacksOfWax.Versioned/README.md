#Versioned API Demo

This project contains an application that demonstrates versioning an ASP.NET Web API project
using route versioning. For other versioning styles, see [Your API versioning is wrong, 
which is why I decided to do it 3 different wrong ways](http://www.troyhunt.com/2014/02/your-api-versioning-is-wrong-which-is.html).

In this demo, there are two versions of the Artist resource that the API supports. The difference is that the second version contains
a count of the number of Albums for the Artist. Versioning is implemented by mapping a route using the template

    api/v{version}/{controller}/{id}

I created two controllers, ArtistsV1Controller and ArtistsV2Controller, and VersionedApiControllerSelector which extends 
DefaultHttpControllerSelector to select the correct controller using the version supplied in the route. VersionedApiControllerSelector 
looks for the controller and version route values and if they're supplied, it builds the controller name, locates its descriptor in
the controller mappings, and returns that.

The application also demonstrates attribute routing in the ArtistsV2 controller GetArtistAlbums method which has a route attribute applied:
    
    api/v2/artists/{id}/albums

In this case, the version is defined in the route so there's no need to locate it in the route data. This is the easy way out, because 
getting the controller name from an attribute route request is a challenge. In this case, SelectController in VersionedApiControllerSelector
just defers to the base method to return the controller descriptor.