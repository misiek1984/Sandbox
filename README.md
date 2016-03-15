# Common
*Common* solution is a collection of libraries, utilities, algorithms, helpers etc. (written in C#) that are used by my other projects. Here is a short description of them:

1. *MK.Compression* - A library that allows one to pack/unpack data. I implemented it before System.IO.Compression.ZipFile class was added o .NET
2. *MK.Data* - This library conains a basic classes and interfaces related to data persistence.
3. *MK.Data.RelationalDB* - A lighweight wrapper over ADO.NET.
4. *MK.Data.Xml*  - A lirabry that makes writing/reading XML data easier.
5. *MK.Dropbox* - A library based on *DropNet* that facilities communication with Dropbox.
6. *MK.Live* - A library based on *Microsoft.Live* facilities communication with SkyDrive.
7. *MK.Logging* - A logging library that is currently based on Enterprise Library Logging Application Block.
8. *MK.MyMath* - A library with a few simple math algoritihms and data structures.
9. *MK.Scripting* - A scripting engine for C# based on *Mono.CSharp*.
10. *MK.Settings* - A library that makes managing settings easily. Firstly you should use ```SettingsProperty``` attribute to to mark properties that should be persisted. Then you should use ```SettingsProvider``` either to extract/object values from/into a given object.
11. *MK.UI.WPF* - Various WPF controls, behaviours and converters. A library also contains MVVM helpers.
12. *MK.Utilities* - Various things that are too small for a seperate library. For example common extension methods, an encryption helper, an object dumber...
13. *WPFLocalizeExtension* - This project is based on the old version of WPFLocalisationExtension project that was available on codeplex (http://wpflocalizeextension.codeplex.com/).
