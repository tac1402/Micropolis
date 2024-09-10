# Micropolis Unity Version 2017

Micropolis is a ground up C# rewrite of the MicropolisCore system using the Unity engine to create a full blown Micropolis native game that runs on Windows, Mac, and Unix. It includes a fully working city simulation game true to the original along with several new features and improvements.

![screenshot](https://github.com/bsimser/micropolis-unity/blob/develop/images/micropolis.png)

## Documentation 2017

Please see our [wiki](https://github.com/bsimser/micropolis-unity/wiki) for everything you wanted to know about Micropolis, but were afraid to ask.

## License

This software is free software and licensed under the [Micropolis GPL License](https://github.com/bsimser/Micropolis/blob/master/LICENSE.md). 

## History

Thursday, January 10, 2008 was the day [Don Hopkins](https://github.com/SimHacker) released Micropolis, an open source release of the original City Simulator, [SimCity](https://en.wikipedia.org/wiki/SimCity). I've been involved with Micropolis ever since the day Don told me about the release of the code. I immediately got into it, helped promote it, fixed up things in the code, and wrote a series of (unfinished) [blog posts](https://weblogs.asp.net/bsimser/building-a-city-the-series) about it.

After all, this was based on the *original* SimCity source code right? How could I not do something with it.

The setup of the original code was hard. It required a ton of tools (SWIG, Tcl, Python, etc.) and resulted in a slow and klunky system that, when a crash occured, it was hard to tell if it was the graphics, the interpreter, the original code, or any one of the dozen or so subsystems in between.

Skip ahead a few years and, well, things haven't gone too far. A few people have ran with the code but it never became the massive thing that the original SimCity ever was (and I didn't think it would have). Instead there are a few ports of it to different platform (JavaScript, C#, etc.) but nothing to write home about.

So here we are with yet-another-port. I guess.

## Project 2017

This rewrite of Micropolis represents a culmination of sources from the original Micropolis source code release which was made up of modified TCL/Tk C code (based on the original X11 Multiplayer Unix release of SimCity, but with the Multiplayer bits removed) and the C++/Python rewrite done by [Don Hopkins](https://github.com/SimHacker). 

Using both of these codebases as a reference, this project builds a C# 2D game using the Unity engine for graphics and UI. The result is a game that runs on modern hardware as a single stand-alone executable with the ability to port and run on other platforms (iOS, Android, etc.). No special tools are required to run it except a computer. The installation is a single click download (with built-in automatic updates so you always have the latest version).

This project can be built using the free version of Unity v2017.3 on any platform the Unity Editor is supported. Just open the project and build. All code and assets are included. Micropolis makes use of the following (free) assets:
* [TextMesh Pro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126)
* [Cinemachine](https://assetstore.unity.com/packages/essentials/cinemachine-79898)

Both assets are free and from Unity Technologies and should be downloaded and installed prior to opening the project in Unity.

## Project 2024
Further development will be carried out by the developer in Russian, you can use a translator.

Спустя более, чем 5 лет, я обнаружил этот заброшенный исходный код. И решил его немного привести к современному виду. Тем более прочитав заметки предыдущих разработчиков увидел, что они так же собирались орефакторить данный код, намериваясь сделать его объектно-ориентированным. Что стало с предыдущими разрабочиками мне не ведомо. Единственно, что происходило после 2017 года, мной была обнаружена ветка с обновлением версии Юнити до 2019. Первое что я сделал это обновил версию юнити 2022.3.16f1. Теперь, все это помещенно в master ветку, и пляски начинаются. Я веду канал на ютубе, на котором буду освещать ход рефакторинга https://www.youtube.com/@hardlandingtac . 

