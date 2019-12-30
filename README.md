# tk7texturevanilla
 A tool for extracting textures from Tekken 7.0 (2015) uasset files.

# Usage
### Via explorer
Drag a uasset file to tk7texturevanilla.exe and the tool will output the extracted texture

### Via command prompt
tk7texturevanilla <uasset-file> - Will extract a texture from the uasset file and output a DDS file where the exe is located
tk7texturevanilla <uasset-file> -o <path> - Will extract a texture from the uasset file and output it to <path>
 
Example:
tk7texturevanilla **"C:\Program Files\Games\TekkenGame\Content\UI\flash\SW_UI_Makuai\Rep_RNK_PROMOTION_M_L.uasset"** -o **"C:\texturefolder"**
will extract the texture from **Rep_RNK_PROMOTION_M_L.uasset** and save the DDS file at **"C:\texturefolder"**
