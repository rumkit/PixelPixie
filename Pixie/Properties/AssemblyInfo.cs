﻿using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using CommandLine;
using CommandLine.Text;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Pixie")]
[assembly: AssemblyDescription("")]
#if DEBUG
[assembly: AssemblyConfiguration("DEBUG")]
#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif
[assembly: AssemblyCompany("RumCode")]
[assembly: AssemblyProduct("Pixie")]
[assembly: AssemblyCopyright("Copyright ©  2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8d1b9483-ef36-451f-8df0-2153cff3b1a4")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.4.*")]                        
[assembly: AssemblyFileVersion("1.4.0.0")]
[assembly: AssemblyInformationalVersion("1.4")]
[assembly: AssemblyUsage(
    "pixie parse someimage.bmp",
    "pixie parse input.bmp --output=array.txt -s -c myconfig.json",
    "pixie generate -w 5 -h 10",
    "pixie generate -w 16 -h 16 -i font.txt\n\0",
    "You can get additional help by using 'pixie parse --help' for example")]
