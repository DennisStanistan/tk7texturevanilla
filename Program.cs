using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static tk7texturevanilla.DirectX.DirectXTexUtility;

namespace tk7texturevanilla
{
    public enum EPixelFormat
    {
        PF_Unknown = 0,
        PF_A32B32G32R32F = 1,
        PF_B8G8R8A8 = 2,
        PF_G8 = 3,
        PF_G16 = 4,
        PF_DXT1 = 5,
        PF_DXT3 = 6,
        PF_DXT5 = 7,
        PF_UYVY = 8,
        PF_FloatRGB = 9,
        PF_FloatRGBA = 10,
        PF_DepthStencil = 11,
        PF_ShadowDepth = 12,
        PF_R32_FLOAT = 13,
        PF_G16R16 = 14,
        PF_G16R16F = 15,
        PF_G16R16F_FILTER = 16,
        PF_G32R32F = 17,
        PF_A2B10G10R10 = 18,
        PF_A16B16G16R16 = 19,
        PF_D24 = 20,
        PF_R16F = 21,
        PF_R16F_FILTER = 22,
        PF_BC5 = 23,
        PF_V8U8 = 24,
        PF_A1 = 25,
        PF_FloatR11G11B10 = 26,
        PF_A8 = 27,
        PF_R32_UINT = 28,
        PF_R32_SINT = 29,
        PF_PVRTC2 = 30,
        PF_PVRTC4 = 31,
        PF_R16_UINT = 32,
        PF_R16_SINT = 33,
        PF_R16G16B16A16_UINT = 34,
        PF_R16G16B16A16_SINT = 35,
        PF_R5G6B5_UNORM = 36,
        PF_R8G8B8A8 = 37,
        PF_A8R8G8B8 = 38,
        PF_BC4 = 39,
        PF_R8G8 = 40,
        PF_ATC_RGB = 41,
        PF_ATC_RGBA_E = 42,
        PF_ATC_RGBA_I = 43,
        PF_X24_G8 = 44,
        PF_ETC1 = 45,
        PF_ETC2_RGB = 46,
        PF_ETC2_RGBA = 47,
        PF_R32G32B32A32_UINT = 48,
        PF_R16G16_UINT = 49,
        PF_ASTC_4x4 = 50,
        PF_ASTC_6x6 = 51,
        PF_ASTC_8x8 = 52,
        PF_ASTC_10x10 = 53,
        PF_ASTC_12x12 = 54,
        PF_BC6H = 55,
        PF_BC7 = 56,
        PF_R8_UINT = 57,
        PF_L8 = 58,
        PF_XGXR8 = 59,
        PF_R8G8B8A8_UINT = 60,
        PF_R8G8B8A8_SNORM = 61,
        PF_R16G16B16A16_UNORM = 62,
        PF_R16G16B16A16_SNORM = 63,
        PF_PLATFORM_HDR_0 = 64,
        PF_PLATFORM_HDR_1 = 65,
        PF_PLATFORM_HDR_2 = 66,
        PF_NV12 = 67,
        PF_R32G32_UINT = 68,
        PF_MAX = 69,
    }

    public struct FTexture2DMipMap
    {
        MemoryStream ms;
        public int Unk;
        public int Unk2;
        public int Size;
        public int Size2;
        public int Unk3;
        public int Unk4;
        public int UnkOffset;
        public int UnkOffset2;

        public FTexture2DMipMap(byte[] array)
        {
            ms = new MemoryStream(array);
            ms.Position = 0;

            // The 'this' object cannot be used before all of its fields are assigned to.
            Unk = 0;
            Unk2 = 0;
            Size = 0;
            Size2 = 0;
            Unk3 = 0;
            Unk4 = 0;
            UnkOffset = 0;
            UnkOffset2 = 0;
            // The 'this' object cannot be used before all of its fields are assigned to...........................

            Unk = ReadInt();
            Unk2 = ReadInt();
            Size = ReadInt();
            Size2 = ReadInt();
            Unk3 = ReadInt();
            Unk4 = ReadInt();
            UnkOffset = ReadInt();
            UnkOffset2 = ReadInt();
        }

        private int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        private byte[] ReadBytes(int size)
        {
            byte[] buffer = new byte[size];
            ms.Read(buffer, 0, size);

            return buffer;
        }
    }

    public struct ProgramAction
    {
        public static List<DictionaryMap<EPixelFormat, DXGIFormat>> FormatMapList = new List<DictionaryMap<EPixelFormat, DXGIFormat>>() {
            { new DictionaryMap<EPixelFormat, DXGIFormat>(EPixelFormat.PF_DXT1, DXGIFormat.BC1UNORM) },
            { new DictionaryMap<EPixelFormat, DXGIFormat>(EPixelFormat.PF_DXT5, DXGIFormat.BC3UNORM) },
            { new DictionaryMap<EPixelFormat, DXGIFormat>(EPixelFormat.PF_BC5, DXGIFormat.BC5UNORM) },
            { new DictionaryMap<EPixelFormat, DXGIFormat>(EPixelFormat.PF_B8G8R8A8, DXGIFormat.B8G8R8A8UNORM) },
            { new DictionaryMap<EPixelFormat, DXGIFormat>(EPixelFormat.PF_FloatRGBA, DXGIFormat.R32G32B32A32FLOAT) }
        };

        public string FilePath;
        public string OutputPath;

        public void Invoke()
        {
            string outPath = string.IsNullOrEmpty(OutputPath) ? $"{Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Path.GetFileNameWithoutExtension(FilePath))}.dds" : $"{Path.Combine(OutputPath, Path.GetFileNameWithoutExtension(FilePath))}.dds";
            byte[] fileData = File.ReadAllBytes(FilePath);

            // I am not going to write an entire class that verifies that this is an actual uasset file so just get the magic number instead.
            if (BitConverter.ToUInt32(fileData.GetBytes(0x0, 0x4), 0x0) != 0x9E2A83C1)
            {
                Console.WriteLine("ERROR: Not a valid uasset file.");
                return;
            }

            // Check if the licensee version is FFFFFFFD (-3)
            if (BitConverter.ToUInt32(fileData.GetBytes(0x4, 0x4), 0x0) != 0xFFFFFFFD)
            {
                Console.WriteLine("ERROR: The file provided is not a Tekken 7.0 (2015) uasset file.");
                return;
            }

            Console.WriteLine("INFO: Finding PF offset...");
            int[] offsets = ArrayExtensions.Locate(fileData, Encoding.ASCII.GetBytes("PF_"));

            if (offsets.Length < 1 || offsets.Length == 0)
            {
                Console.WriteLine("ERROR: No texture was found.");
                return;
            }

            int dataStartOffset = BitConverter.ToInt32(fileData.GetBytes(0x93, 0x4), 0x0);
            int pfOffset = offsets[1];

            Console.WriteLine("INFO: PF offset was found, getting texture info...");

            EPixelFormat pixelFormat = EPixelFormat.PF_Unknown;
            if(!Enum.TryParse(fileData.GetNullTerminatedString(pfOffset), out pixelFormat))
            {
                Console.WriteLine("ERROR: Could not retrieve texture's pixel format.");
                return;
            }

            int width = BitConverter.ToInt32(fileData.GetBytes(pfOffset - 0x10, 0x4), 0x0);
            int height = BitConverter.ToInt32(fileData.GetBytes(pfOffset - 0xC, 0x4), 0x0);

            // NOTE TO SELF: Get rid of this awful pixel format offset calculation mess that is happening here and write something proper
            FTexture2DMipMap mipMapData = new FTexture2DMipMap(fileData.GetBytes(pfOffset + 0x10 + 
                (pixelFormat == EPixelFormat.PF_B8G8R8A8 ? 0x4 : 0x0) + // pixel format can't be BC5 and BGRA8 at the same time so it's fine
                (pixelFormat == EPixelFormat.PF_BC5 ? -0x1 : 0x0) // it's fine, i promise!
                , 20));

            Console.WriteLine($"INFO: Texture resolution is {width}x{height} with the size of 0x{mipMapData.Size:X2} - Pixel Format: {pixelFormat.ToString()}");

            byte[] textureData = fileData.GetBytes(dataStartOffset, mipMapData.Size);

            Console.WriteLine("INFO: Generating a DDS file...");

            DXGIFormat dxgiFormat = FormatMapList.Find(map => map.x == pixelFormat).y;
            DDSFlags flags = DDSFlags.NONE;

            TexMetadata x = GenerateMataData(width, height, 0, dxgiFormat, false);
            DDSHeader ddsHeader;
            DX10Header dx10Header;
            dx10Header.Format = dxgiFormat;
            GenerateDDSHeader(x, flags, out ddsHeader, out dx10Header);

            List<byte> ddsData = new List<byte>();
            ddsData.AddRange(EncodeDDSHeader(ddsHeader, dx10Header));
            ddsData.AddRange(textureData);

            try
            {
                File.WriteAllBytes(outPath, ddsData.ToArray());
            }
            catch(DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                File.WriteAllBytes(outPath, ddsData.ToArray());
            } 
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            Console.WriteLine($"Extracted {Path.GetFileName(FilePath)} ({pixelFormat.ToString()}) -> {Path.GetFileName(outPath)}");
        }
    }

    class Program
    {
        public static void OutputHelp()
        {
            Console.WriteLine("Usage: tk7texturevanilla [uasset-file-path] [options]");
            Console.WriteLine("-o <path> Output the texture to a specified path");
        }

        private static void Main(string[] args)
        {
            // Where the mess begins.

            if (args.Length == 0)
            {
                OutputHelp();
            }

            if(args.Length == 1)
            {
                if (File.Exists(args[0]))
                {
                    ProgramAction programAction = new ProgramAction();
                    programAction.FilePath = args[0];
                    programAction.Invoke();
                }
                else OutputHelp();
            }

            if(args.Length == 3)
            {
                if (File.Exists(args[0]))
                {
                    ProgramAction programAction = new ProgramAction();
                    programAction.FilePath = args[0];

                    switch (args[1])
                    {
                        case "-o":
                            programAction.OutputPath = (string)args[2];
                            break;

                        default:
                            Console.WriteLine("ERROR: Invalid option provided.");
                            return;
                    }

                    programAction.Invoke();
                }
            }
        }
    }
}
