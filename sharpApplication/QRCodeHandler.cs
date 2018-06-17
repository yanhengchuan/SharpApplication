using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;

namespace SMSGroup.Utility
{
    /// <summary>  
    /// 二维码处理类  
    /// </summary>  
    public class QRCodeHandler
    {
        /// <summary>  
        /// 创建二维码  
        /// </summary>  
        /// <param name="QRString">二维码字符串</param>  
        /// <param name="QRCodeEncodeMode">二维码编码(Byte、AlphaNumeric、Numeric)</param>  
        /// <param name="QRCodeScale">二维码尺寸(Version为0时，1：26x26，每加1宽和高各加25</param>  
        /// <param name="QRCodeVersion">二维码密集度0-40</param>  
        /// <param name="QRCodeErrorCorrect">二维码纠错能力(L：7% M：15% Q：25% H：30%)</param>  
        /// <param name="filePath">保存路径</param>  
        /// <param name="hasLogo">是否有logo(logo尺寸50x50，QRCodeScale>=5，QRCodeErrorCorrect为H级)</param>  
        /// <param name="logoFilePath">logo路径</param>  
        /// <returns></returns>  
        public bool CreateQRCode(string QRString, string QRCodeEncodeMode, short QRCodeScale, int QRCodeVersion, string QRCodeErrorCorrect, string filePath, bool hasLogo, string logoFilePath)
        {
            bool result = true;

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

            switch (QRCodeEncodeMode)
            {
                case "Byte":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case "AlphaNumeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case "Numeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            qrCodeEncoder.QRCodeScale = QRCodeScale;
            qrCodeEncoder.QRCodeVersion = QRCodeVersion;

            switch (QRCodeErrorCorrect)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                case "H":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }

            try
            {
                Image image = qrCodeEncoder.Encode(QRString, System.Text.Encoding.UTF8);
                System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                fs.Close();

                if (hasLogo)
                {
                    Image copyImage = System.Drawing.Image.FromFile(logoFilePath);
                    Graphics g = Graphics.FromImage(image);
                    int x = image.Width / 2 - copyImage.Width / 2;
                    int y = image.Height / 2 - copyImage.Height / 2;
                    g.DrawImage(copyImage, new Rectangle(x, y, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                    g.Dispose();

                    image.Save(filePath);
                    copyImage.Dispose();
                }
                image.Dispose();

            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

    }

    /// <summary>  
    /// 生成条形码( 128条码,标准参考:GB/T 18347-2001 )  

    /// BY JUNSON 20090508  
    /// </summary>  

    public class BarCode
    {
        /// <summary>  
        /// 条形码生成函数  
        /// </summary>  
        /// <param name="text">条型码字串</param>  
        /// <returns></returns>  
        public static void CreateOneBarCode(string text, string barImageOutPath)
        {
            //查检是否合条件TEXT  
            bool ck = CheckErrerCode(text);
            if (!ck)
            {
                System.Windows.Forms.MessageBox.Show("条形码字符不合要求，不能是存在汉字或全角字符");
                return;
            }
            string barstring = BuildBarString(text);
            Bitmap image = KiCode128C(barstring, 30);
            System.IO.FileStream fs = new System.IO.FileStream(barImageOutPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();
            image.Dispose();

        }

        /// <summary>  
        /// 建立条码字符串  
        /// </summary>  
        /// <param name="tex">条码内容</param>  
        /// <returns></returns>  
        private static string BuildBarString(string tex)
        {
            string barstart = "bbsbssbssss";    //码头  
            string barbody = "";                //码身  
            string barcheck = "";               //码检  
            string barend = "bbsssbbbsbsbb";    //码尾  

            int checkNum = 104;
            //循环添加码身,计算码检  
            for (int i = 1; i <= tex.Length; i++)
            {
                int index = (int)tex[i - 1] - 32;
                checkNum += (index * i);

                barbody += AddSimpleTag(index);//加入字符值的条码标记  
            }
            //码检值计算  

            barcheck = AddSimpleTag(int.Parse(Convert.ToDouble(checkNum % 103).ToString("0")));


            string barstring = barstart + barbody + barcheck + barend;
            return barstring;
        }

        //增加一个条码标记  
        private static string AddSimpleTag(int CodeIndex)
        {
            string res = "";

            /// <summary>1-4的条的字符标识 </summary>  
            string[] TagB = { "", "b", "bb", "bbb", "bbbb" };
            /// <summary>1-4的空的字符标识 </summary>  
            string[] TagS = { "", "s", "ss", "sss", "ssss" };
            string[] Code128List = new string[] {  
                "212222","222122","222221","121223","121322","131222","122213","122312","132212","221213",  
                "221312","231212","112232","122132","122231","113222","123122","123221","223211","221132",  
                "221231","213212","223112","312131","311222","321122","321221","312212","322112","322211",  
                "212123","212321","232121","111323","131123","131321","112313","132113","132311","211313",  
                "231113","231311","112133","112331","132131","113123","113321","133121","313121","211331",  
                "231131","213113","213311","213131","311123","311321","331121","312113","312311","332111",  
                "314111","221411","431111","111224","111422","121124","121421","141122","141221","112214",  
                "112412","122114","122411","142112","142211","241211","221114","413111","241112","134111",  
                "111242","121142","121241","114212","124112","124211","411212","421112","421211","212141",  
                "214121","412121","111143","111341","131141","114113","114311","411113","411311","113141",  
                "114131","311141","411131","211412","211214","211232" };

            string tag = Code128List[CodeIndex];

            for (int i = 0; i < tag.Length; i++)
            {
                string temp = "";
                int num = int.Parse(tag[i].ToString());
                if (i % 2 == 0)
                {
                    temp = TagB[num];
                }
                else
                {
                    temp = TagS[num];
                }
                res += temp;
            }
            return res;
        }

        /// <summary>  
        /// 检查条形码文字是否合条件(不能是汉字或全角字符)  
        /// </summary>  
        /// <param name="cktext"></param>  
        /// <returns></returns>  
        private static bool CheckErrerCode(string cktext)
        {
            foreach (char c in cktext)
            {
                byte[] tmp = System.Text.UnicodeEncoding.Default.GetBytes(c.ToString());

                if (tmp.Length > 1)
                    return false;
            }
            return true;
        }

        /// <summary>生成条码 </summary>  
        /// <param name="BarString">条码模式字符串</param>  //Format32bppArgb  
        /// <param name="Height">生成的条码高度</param>  
        /// <returns>条码图形</returns>  
        private static Bitmap KiCode128C(string BarString, int _Height)
        {

            Bitmap b = new Bitmap(BarString.Length, _Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //using (Graphics grp = Graphics.FromImage(b))  
            //{  
            try
            {
                char[] cs = BarString.ToCharArray();


                for (int i = 0; i < cs.Length; i++)
                {
                    for (int j = 0; j < _Height; j++)
                    {
                        if (cs[i] == 'b')
                        {
                            b.SetPixel(i, j, System.Drawing.Color.Black);
                        }
                        else
                        {
                            b.SetPixel(i, j, System.Drawing.Color.White);
                        }
                    }
                }

                //grp.DrawString(text, SystemFonts.CaptionFont, Brushes.Black, new PointF(leftEmpty, b.Height - botEmpty));  

                return b;
            }
            catch
            {
                return null;
            }
            //}  
        }

    }  
}