# Kaleidoscope

## Usage:
https://uberyoji.github.io/Kaleidoscope/

| Param         | Type    | Purpose  |
|:--------------|:-------:|:-----|
| slicecount    | integer | Sets number of slice            |
| scrollspeed   | float   | Speed at which uvs are scrolled |
| rotationspeed | float   | Speed at which uvs are rotated  |
| offsetx | float   | Texture offset  |
| rotationspeed | float   | Speed at which uvs are rotated  |
| rotationspeed | float   | Speed at which uvs are rotated  |
| rotationspeed | float   | Speed at which uvs are rotated  |
| url           | string  | Url of image to load. Url must be encoded. Image must be power of two.  |


Ex: https://uberyoji.github.io/Kaleidoscope/?slicecount=32&scrollspeed=-0.1&rotationspeed=0.25

## Know issue
If you try to use an url to an image, and the remote server does not have CORS set up or configured correctly, you will see an error like this in the browser console:
Cross-Origin Request Blocked: The Same Origin Policy disallows reading the remote resource at http://myserver.com/. This can be fixed by moving the resource to the same domain or enabling CORS.

See unity documentation on how to fix this if you are hosting your own images.
https://docs.unity3d.com/Manual/webgl-networking.html
