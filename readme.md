# DirLister v2


### Features
* Multi-format output: HTML, plain text (.txt), CSV, JSON, XML, Markdown (.md). See output examples [here](https://github.com/SanderSade/DirLister/tree/master/docs)
* Filtering by filename (multiple wildcards or regular expression)
* Shell integration -  can be run from right-click menu for folders and drives, with or without opening UI
* Can include file sizes
* Option to include media info
* Option to include file creation and modified dates
* Can include system and hidden files
* Drag'n'drop support
* Fast, free, open source

#### Screenshots
Click on image to enlarge

![Input tab](https://user-images.githubusercontent.com/18664267/53328900-a4868b00-38f3-11e9-973b-4bc4d91cf187.png){:height="50%" width="50%"}![Output tab](https://user-images.githubusercontent.com/18664267/53328947-b9fbb500-38f3-11e9-8b96-7a6a0419d027.png){:height="50%" width="50%"}
![Shell integration](https://user-images.githubusercontent.com/18664267/53329865-cd0f8480-38f5-11e9-812d-44831277ea68.png){:height="50%" width="50%"}![Optional progress window](https://user-images.githubusercontent.com/18664267/53329283-6d64a980-38f4-11e9-9a38-3c9aed74829f.png){:height="50%" width="50%"}


### TODO
* Win32 Installer
* UWP installer
* Hints
* About tab contents
* Improve JSON format?

#### Advanced
While most of the options can be set from UI, there are some advanced options that are available only by manually editing the configuration file (user.config).
* History:
	* DirectoryHistoryLength - maximum number of directory history entries. Defaults to 16
	* FilterHistoryLength - maximum number of filter entries (separate for wildcards and regex). Defaults to 8
* Settings:
	* EnableMultithreading - True/False, defaults to False. Set to true to enable multi-threading for file info gathering and media processing. Should never be enabled for regular hard drives, as it will cause HDD trashing and slow down list creation. Can speed up processing on SSDs.
	* DateFormat - date/time output format for output. Defaults to yyyy-MM-dd HH:mm:ss (ISO 8601). See https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings  for options.
	* CssFile - specify custom CSS file to include to HTML output instead of the default CSS. This is filename, not content - and ignored if the file doesn't exist.

### Used components

* Code from [taglib#](https://github.com/mono/taglib-sharp), licensed under [GNU Lesser General Public License v2.1](https://github.com/mono/taglib-sharp/blob/master/COPYING). Heavily modified.


### Version history
* 2.0 Complete rewrite