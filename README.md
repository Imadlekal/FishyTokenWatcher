# Fishy Token Watcher
<br>Check money flow with ethplorer.io API
<br>======================================
<br>
<br>This application creates a tray icon and will check your wallet for changes = Ethereum or tokens received or spent.
<br>When a change is detected a notification balloon will pop up.
<br>The application is made in C# and compiled for Windows. Make sure the notifications are enabled if you want to use it.
<br>
<br>Limitations:
<br>============
<br>I don't know how the application behaves if too many token types are sent/received in the period between 2 reads.
<br>I don't know how the application behaves if too many token types are in the wallet (the data is saved in settings)
<br>
<br><b>Please read the limits for the API keys at https://github.com/EverexIO/Ethplorer/wiki/Ethplorer-API</b>
<br>Initially you can use the free API key (<a href="https://github.com/EverexIO/Ethplorer/wiki/Ethplorer-API">go there, find it, and also check the limitations!</a>) or you can apply for a personal key (also there, also for free).
<br>
<br>Liability:
<br>=========
<br>I am not responsible for any loses. The code is there, look into it, run it or not.
<br>Obviously the application needs more testing and probably updates. Keep in mind that it's free and young (i.e. may have bugs).
<br>
<br>
<br>Many thanks to https://ethplorer.io/ for providing such a handy API to everyone.
<br>
<br>
<br>Executable:
<br>============
<br>You should not trust me blindly and just run it, you should clearly build it yourself (Visual Studio 2017/.NET 4.5)
<br>However, the exe is available, archived in the Binaries folder.
<br>
<br>
