# Developer Notes

## CDN

Once the cdn has gained certain features, we should be able to serve some content direct from the CDN. (We already pick up images and fonts from it.)

We need the files in the CDN to include a version in their name (or path), and we need minified versions in there. We could then serve GOV.UK's (and when it's ready common DAS and our site's specific) css and js from the CDN.

We might also want to bundle some or all of the GOV.UK / common DAS / site specific artifacts together, but only really if we can populate/update the cdn as part of the release pipeline. We should aim to not go above the [concurrent number of downloads per server](https://stackoverflow.com/questions/985431/max-parallel-http-connections-in-a-browser) for modern browsers, to keep page latency low when artifacts are not cached. To help with that aim we might want to combine GOV.UK's images into a sprite sheet.
