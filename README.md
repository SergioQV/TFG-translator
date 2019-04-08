# TFG-translator
This projects aims to achieve a speech recognition in streaming, using the default voice input, to be used in a unity project.

To do this, it's necesary to:
1- Compile the project in visual studio as class library (will be generated as ``ReconocimientoDeVoz.dll`` and there'll be too a ``.xml`` file and a ``.pdb`` file with the same name)
2- Copy the result that is in ``ReconocimientoDeVoz/bin/Release`` and all the libraries it depends on and put it in the Unity ``Assets`` folder (for example, in a folder called ``Assets/Plugins``).
3- Once in there, copy the Grpc.Core folder into the same Plugins. It have the ``grpc_csharp_ext.dll`` libraries divided by the bits of the OS and by the OS itself (windows, osx, etc...)


Once it's done, your Unity project is set up for working.
