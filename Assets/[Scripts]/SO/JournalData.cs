using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SDM_FythmPipe
{
    [CreateAssetMenu(menuName = "Scriptable/Journal", fileName ="Journal Data")]
    public class JournalData : ScriptableObject
    {

        [SerializeField]
        public Texture2D picsTaken { get; set; }

        [SerializeField]
        public int beforeCount;

        [SerializeField]
        public int afterCount;

        [SerializeField]
        public bool hasCapturedBefore;

        public List<JournalDate> dates;

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
            ResetValues();
        }
        private void OnDisable()
        {
            ResetValues();
        }
        private void ResetValues()
        {
            hasCapturedBefore = false;
            dates.Clear();
            beforeCount = 0;
            afterCount = 0;
        }
  

        public IEnumerator IeTakePic(Texture pic)
        {
            yield return new WaitForEndOfFrame();

            int width = pic.width;
            int height = pic.height;

            Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            RenderTexture currentRT = RenderTexture.active;

            RenderTexture renderTexture = new RenderTexture(width, height, 32);
            Graphics.Blit(pic, renderTexture);

            RenderTexture.active = renderTexture;

            screenshotTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            screenshotTexture.Apply();

            //Color[] pixels = screenshotTexture.GetPixels();

            RenderTexture.active = currentRT;

            picsTaken = screenshotTexture;
            FlipTextureVertically(picsTaken);


            Destroy(renderTexture);
        }

        Texture2D RotateTexture(Texture2D originalTexture, bool clockwise)
        {
            Color32[] original = originalTexture.GetPixels32();
            Color32[] rotated = new Color32[original.Length];
            int w = originalTexture.width;
            int h = originalTexture.height;

            int iRotated, iOriginal;

            for (int j = 0; j < h; ++j)
            {
                for (int i = 0; i < w; ++i)
                {
                    iRotated = (i + 1) * h - j - 1;
                    iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                    rotated[iRotated] = original[iOriginal];
                }
            }

            Texture2D rotatedTexture = new Texture2D(h, w);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            return rotatedTexture;
        }

        public static void FlipTextureVertically(Texture2D original)
        {
            var originalPixels = original.GetPixels();

            var newPixels = new Color[originalPixels.Length];

            var width = original.width;
            var rows = original.height;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < rows; y++)
                {
                    newPixels[x + y * width] = originalPixels[x + (rows - y - 1) * width];
                }
            }

            original.SetPixels(newPixels);
            original.Apply();
        }
        void CreateBaseDirectory()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (!Directory.Exists(Application.persistentDataPath + "/AlbumPhoto"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/AlbumPhoto");
                }
            }
            else
            {
                if (!Directory.Exists(Application.dataPath + "/AlbumPhoto"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/AlbumPhoto");
                }
            }
        }
        public void ConfirmPic(AlbumPhotoType albumPhotoType, DateTime dateTime)
        {

            string albumType = "";
            switch (albumPhotoType)
            {
                case AlbumPhotoType.BEFORE:
                    albumType = "Before";
                    break;
                case AlbumPhotoType.AFTER:
                    albumType = "After";
                    break;
                default:
                    break;
            }

            string dateTimeString = dateTime.ToString("dd_MM_yyyy");
            string path = "";
            CreateBaseDirectory();
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                path = Application.persistentDataPath + "/AlbumPhoto/" + albumType;
            else
                path = Application.dataPath + "/AlbumPhoto/" + albumType;


            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            DirectoryInfo info;
            byte[] byteArray;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                FlipTextureVertically(picsTaken);
                byteArray = RotateTexture(picsTaken, true).EncodeToPNG();

            }
            else
            {
                byteArray = RotateTexture(picsTaken, false).EncodeToPNG();
            }

            System.IO.File.WriteAllBytes(path + "/" + dateTimeString + ".png", byteArray);
            info = new DirectoryInfo(path+"/");

            switch (albumPhotoType)
            {
                case AlbumPhotoType.BEFORE:
                    beforeCount = info.GetFiles("*.png").Length;
                    beforeCount = beforeCount++;
                    break;
                case AlbumPhotoType.AFTER:
                    afterCount = info.GetFiles("*.png").Length;
                    afterCount = afterCount++;
                    break;
                default:
                    break;
            }
      
        }
        public Texture GetPicCache() => picsTaken;
        public Texture GetPhotoAlbum(AlbumPhotoType albumPhotoType, DateTime dateTime)
        {
            string dateTimeString = dateTime.ToString("dd_MM_yyyy");
            string albumType = "";

            switch (albumPhotoType)
            {
                case AlbumPhotoType.BEFORE:
                    albumType = "Before";
                    break;
                case AlbumPhotoType.AFTER:
                    albumType = "After";
                    break;
                default:
                    break;
            }


            string path = "/AlbumPhoto/" + albumType.ToString() + "/" + dateTimeString + ".png";
            string fileLocation;
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                fileLocation = Application.persistentDataPath + path;
            else
                fileLocation = Application.dataPath + path;

            FileStream fs;

            try
            {
                fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                return (null);
            }


            byte[] imageData = new byte[fs.Length];
            fs.Read(imageData, 0, (int)fs.Length);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            return (texture);
        }
        public int GetCount(AlbumPhotoType albumPhotoType) {
            switch (albumPhotoType)
            {
                case AlbumPhotoType.BEFORE:
                    return beforeCount;
                case AlbumPhotoType.AFTER:
                    return afterCount;
            }
            return 0;
        }
        public void SetHasCapturedBefore(bool has)=> hasCapturedBefore = has;
        public bool GetHasCapturedBefore() => hasCapturedBefore;
        public enum AlbumPhotoType
        {
            BEFORE,
            AFTER
        }
    }

    [System.Serializable]
    public struct JournalDate
    {
        public string date;
        public string day;
    }
}