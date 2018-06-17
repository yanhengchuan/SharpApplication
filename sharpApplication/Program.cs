using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SMSGroup.Utility;

namespace sharpApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            TestUtil.IsNumberByReg("");

            Type fatherType = typeof(FatherCls);
            Type sonType = typeof(SonClsA);

            SonClsA son1 = new SonClsA ();
            SonClsB son2 = new SonClsB();


            if (fatherType == son1.GetType())
                System.Console.WriteLine("type {0} equal to {1}", fatherType, son1.GetType());
            else
                System.Console.WriteLine("type {0} dose not equal to {1}", fatherType, son1.GetType());

            if (fatherType.IsInstanceOfType(son1))
                System.Console.WriteLine("type {1} is child of {0}", fatherType, son1.GetType());
            else
                System.Console.WriteLine("type {0} dose not equal to {1}", fatherType, son1.GetType());

            if (sonType.IsInstanceOfType(son1))
                System.Console.WriteLine("type {1} is defintion for instance of {0}", son1, sonType);
            else
                System.Console.WriteLine("type {0} dose not equal to {1}", fatherType, son1.GetType());

            if (son1.GetType() != son2.GetType())
                Console.WriteLine("type {0} dose not equal to {1}", son1.GetType(), son2.GetType());

            if(fatherType.IsAssignableFrom(son1.GetType()))
                Console.WriteLine("type {0} is the father to {1}", fatherType, son1.GetType());

            if(fatherType.IsAssignableFrom(sonType))
                Console.WriteLine("type {0} is the father to {1}", fatherType, sonType);

            QRCodeHandler qr = new QRCodeHandler();
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"/QRCode/";    //文件目录  
            string qrString = "SMS Group - Rizhao CCM3";                         //二维码字符串  
            string logoFilePath = path + "SMS-Group.png";                                    //Logo路径50*50  
            string filePath = path + "SlabQRCodeWithSMS.jpg";                                        //二维码文件名  
            qr.CreateQRCode(qrString, "Byte", 5, 0, "H", filePath, true, logoFilePath);   //生成  

            filePath = path + "SlabQRCodeWithSMS1.jpg";                                        //二维码文件名  
            qr.CreateQRCode(qrString, "Byte", 5, 0, "H", filePath, true, null);   //生成  

            filePath = path + "SlabBar.png";
            BarCode.CreateOneBarCode("SMSGroup Slab", filePath);

            logoFilePath = path + "SlabBar.png";
            filePath = path + "SlabQRCodeWithBarCode.jpg";
            qr.CreateQRCode(qrString, "Byte", 5, 0, "H", filePath, true, logoFilePath); 
        }
    }
     
}
