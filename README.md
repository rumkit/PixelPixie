# PixelPixie   [![Build Status](https://travis-ci.com/rumkit/PixelPixie.svg?token=gb3qD7dHcbK4DVMALU87&branch=master)](https://travis-ci.com/rumkit/PixelPixie)

<img src="http://imgur.com/download/yZ9QrRM" width="128">

PixelPixie is a .NET 4.6 console application. It generates a c-style char array from a bitmap grid with graphical fonts.

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