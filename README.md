# TFG-translator
This projects aims to achieve a speech recognition in streaming, using the default voice input, to be used in a unity project.

Instructions:

1- Open the solution with visual studio.

2- Once in visual studio, right click on the project, select ``Manage NuGet packages for solution``, go to examine and search ``Google.Cloud.Speech.V1`` and ``NAudio`` packages. (they're not in this repository because off the size limit)

3- In Visual Studio, open the contextual menu for References in the Solution Explorer and choose Add Reference. Then, choose the option Browse > Browse > select file.

4- Select the library ``Program Files\Unity\Editor\Data\Managed\UnityEngine.dll``

5- Compile the project in visual studio as class library (will be generated as ``ReconocimientoDeVoz.dll`` and there'll be too a ``.xml`` file and a ``.pdb`` file with the same name)

6- Copy the result that is in ``ReconocimientoDeVoz/bin/Release`` and all the libraries it depends on and put it in the Unity ``Assets`` folder (for example, in a folder called ``Assets/Plugins``).

7- Once in there, you must:

  7.1- Go to the ``Packages`` folder in this project (it'll be created after downloading the NuGet packages)

  7.2- Copy the folder ``Grpc.Core.[version]`` into the same assets folder as the other libraries

  7.3- Inside ``Grpc.Core`` folder, delete all folders except ``lib`` and ``runtimes`` (but delete the content of ``lib``)

  7.4- In ``runtime\win\native`` you'll see 2 libraries with the same name but the end (one is x64 and the other ir x86). You should create two folders (one called x64 and the other x86) copy the related file into it and change the two files name, leaving them with the name ``grpc_csharp_ext.dll``.
 
 this is done because when you copy the folder into an assets folder, unity editor gives errors because it finds repeated names inside the assets folder, but you cannot simply delete all the repeated. Those two must stay. 


Once it's done, your Unity project is set up for working.
