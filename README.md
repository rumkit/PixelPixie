# PixelPixie [![Build Status](https://travis-ci.org/rumkit/PixelPixie.svg?branch=master)](https://travis-ci.org/rumkit/PixelPixie)

<img src="https://raw.githubusercontent.com/rumkit/PixelPixie/master/pixie.png" width="128">

#### Motivation

Let's imagine you are an embedded programmer and you want to play with some kind of LCD screen. You want to print some text and that is suddenly hard, because you can only draw pixels. You can't use .ttf fonts because you have no OS in your device (and even if you have, it might have no concept of 'files' alltogether).

So you have googled something like "bitmap fonts for embedded lcd screen arduino" and whooaa that's really a lot of them! And of course none of them is acceptable. It's always like that. Maybe you need 5x7 font but all you see is either 5x8 or 6x7. Maybe you are looking for non-latin font. 
Or maybe you are dealing with something like MAX7456 for making OSD overlay and suddenly you need _two_ bits for each pixel, not one. Or maybe you need three bits for each pixel. Or four.

And here is a solution for you problem! PixelPixie is a bitmap font generator that doesn't use hardcoded bits per pixel value or hardcoded symbol size. With PixelPixie you can finally forget that exhausting hours of drawing bitmap font on a piece of paper and then transforming it in hex by hand. Instead you can use generated png with grid and use your preferred graphical editor. And that should be better, right?


#### Usage

```
Pixie 1.2
Copyright c  2016
Usage: pixie parse someimage.bmp
       pixie parse input.bmp --output=array.txt -c myconfig.json
       pixie generate -w 5 -h 10

  generate    generates a graphical pattern

  parse       parses  a pattern filled with graphical font to a byte array
```

```
pixie generate

  -w, --width     grid pattern width in symbols

  -h, --height    grid pattern height in symbols

  -o, --output    (Default: output.png) output file name

  -c, --config    (Default: config.json) configuration file path
```

```
pixie parse

  -s, --single-array    place all characters to single array

  -o, --output          (Default: output.txt) output file name

  -c, --config          (Default: config.json) configuration file path
```

#### Configuration

All configuration is done via JSON config file. Please note that comments are not allowed normally in JSON and are listed here only as reference.

```YAML
{
  "BitsPerPixel": 2,				// How many bits per pixel are used in a result array
  "SymbolWidth": 12,				// One symbol dimensions in pixels
  "SymbolHeight": 18,				//
  "DelimeterWidth": 1,				// Symbol delimeter width and height
  "DelimeterHeight": 1,				//
  "DelimeterColor": "#FF0000",		// Color of delimeter in generated pattern
  "ColorMapping": {					// A dictionary that is used to map a color in bitmap to
    "#FFFFFF": 0,					// a bit sequence. Bit sequence is stored in DEC. And
    "#000000": 1,					// only first N bits (BitsPerPixel) are taken in account
    "#969696": 2
  }
}
```

``` DelimeterColor  ``` parameter only applies to empty grid generation, as well as ``` ColorMappings ``` are used only while parsing graphical font. Default config.json  can be found [here](https://github.com/rumkit/PixelPixie/blob/master/Pixie/config.json).
