# PixelPixie   [![Build Status](https://travis-ci.com/rumkit/PixelPixie.svg?token=gb3qD7dHcbK4DVMALU87&branch=master)](https://travis-ci.com/rumkit/PixelPixie)

<img src="http://imgur.com/download/yZ9QrRM" width="128">

#### Motivation

Let's imagine you are an embedded programmer and you want to play with some kind of LCD screen. You want to print some text and that is suddenly hard, because you can only draw pixels. You can't use .ttf fonts because you have no OS in your device (and even if you have, it might have no concept of 'files' alltogether).

So you have googled something like "bitmap fonts for embedded lcd screen arduino" and whooaa that's really a lot of them! And of course none of them is acceptable. It's always like that. Maybe you need 5x7 font but all you see is either 5x8 or 6x7. Maybe you are looking for non-latin font. 
Or maybe you are dealing with something like MAX7456 for making OSD overlay and suddenly you need _two_ bits for each pixel, not one. Or maybe you need three bits for each pixel. Or four.

And here is a solution for you problem! PixelPixie is a bitmap font generator that doesn't use hardcoded bits per pixel value or hardcoded symbol size. With PixelPixie you can finally forget that exhausting hours of drawing bitmap font on a piece of paper and then transforming it in hex by hand. Instead you can use generated png with grid and use your preferred graphical editor. And that should be better, right?


#### Usage

```
Pixie 1.0
Copyright c  2016
Usage: pixie --input=input.bmp
       pixie -i input.bmp -o array.txt -c myconfig.json
       pixie -g -o pattern.png -w 5 -h 10

  -s, --single-array    place all characters to single array

  -c, --config          (Default: config.json) configuration file path

  -o, --output          Required. output file name

  -i, --input           output file name

  -g, --generate        generate grid pattern

  -w, --width           grid pattern width in symbols

  -h, --height          grid pattern height in symbols

  --help                Display this help screen.
```



#### Configuration

All configuration is done via JSON config file:

```json
{
  "BitsPerPixel": 2,
  "SymbolWidth": 12,
  "SymbolHeight": 18,
  "DelimeterWidth": 1,
  "DelimeterHeight": 1,
  "DelimeterColor": "#FF0000",
  "ColorMapping": {
    "#FFFFFF": 0,
    "#000000": 1,
    "#969696": 2
  }
}
```

``` DelimeterColor  ``` parameter only applies to empty grid generation, as well as ``` ColorMappings ``` are used only while parsing graphical font. Default config.json  can be found [here](https://github.com/rumkit/PixelPixie/blob/master/Pixie/config/config.json).