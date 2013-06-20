GameServer
==========

Creating the base for a multiplayer networked game. This will start as the basic client / server components 
and progress from there.

Trying to get the base component down so that later down the road I do not have to rewrite a lot of code 
to get a single player game to be multiplayer. This way I keep multiplayer in mind and mold my code to fit
that style.


The Libraries I am currently using are:

Lidgren Network Library: http://code.google.com/p/lidgren-network-gen3/

FuchsGUI: http://ghoshehsoft.wordpress.com/2011/02/11/xna-fuchsgui-part-i/

as of 06/20/2013 I have ported this project over to use MonoGame I had to
rebuild LidgrenXNAExtensions to use MonoGame as well as FuchsGUI.

Still having trouble gettign MonoGame Content project to work right so I
went with https://xnacontentcompiler.codeplex.com/ to compile the assets
into xnb format, and uploaded them into the MonoGame Content folder in my
game project.

