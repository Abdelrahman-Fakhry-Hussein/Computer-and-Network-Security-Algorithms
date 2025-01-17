﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {


        byte[,] s_SB = new byte[16, 16]
             {
      {0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76},
      {0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0},
      {0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15},
      {0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75},
      {0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84},
      {0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf},
      {0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8},
      {0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2},
      {0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73},
      {0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb},
      {0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79},
      {0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08},
      {0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a},
      {0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e},
      {0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf},
      {0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16}
             };
        byte[,] I_SB = new byte[16, 16] {
            { 0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb },
            { 0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb },
            { 0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e },
            { 0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25 },
            { 0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92 },
            { 0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84 },
            { 0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06 },
            { 0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b },
            { 0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73 },
            { 0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e },
            { 0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b },
            { 0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4 },
            { 0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f },
            { 0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef },
            { 0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61 },
            { 0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d }
        };

        byte[,] R_mat = new byte[4, 10]
        {
        {0x01,0x02,0x04,0x08,0x10,0x20,0x40,0x80,0x1b,0x36},
        {0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00},
        {0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00},
        {0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}
        };


        public override string Decrypt(string cipherText, string key)
        {




            List<List<Byte>> keys = split_text(key);
            List<List<Byte>> cipherTexts = split_text(cipherText);


            List<List<List<Byte>>> keys_val = new List<List<List<Byte>>>();
            List<List<Byte>> keysCopy = new List<List<Byte>>();
            for (int h = 0; h < keys.Count; h++)
            {
                List<Byte> Te = new List<Byte>();
                for (int hh = 0; hh < 4; ++hh)

                    Te.Add(keys[h][hh]);
                keysCopy.Add(Te);
            }

            keys_val.Add(keysCopy);

            for (int i = 0; i <= 9; i++)
            {
                keys = Keysec(i, keys);
                keysCopy = new List<List<Byte>>();
                for (int h = 0; h < keys.Count; h++)
                {
                    List<Byte> Te = new List<Byte>();
                    for (int hh = 0; hh < 4; ++hh)

                        Te.Add(keys[h][hh]);
                    keysCopy.Add(Te);
                }
                keys_val.Add(keysCopy);

            }




            cipherTexts = AddRoun(cipherTexts, keys);

            for (int o = 9; o > 0; o--)
            {

                cipherTexts = Irotate(cipherTexts);
                cipherTexts = ISB(cipherTexts);
                cipherTexts = AddRoun(cipherTexts, keys_val[o]);
                cipherTexts = IMix_Col(cipherTexts);
            }
            cipherTexts = Irotate(cipherTexts);
            cipherTexts = ISB(cipherTexts);
            cipherTexts = AddRoun(cipherTexts, keys_val[0]);




            return B_to_S(cipherTexts);
        }
        public List<byte> MM(byte tem)
        {
            List<byte> t = new List<byte>(8);
            t.Add(tem);
            for (int i = 1; i < 8; i++)
            {
                if (((byte)(t[i - 1] & 0x80)) == 0x80)
                {
                    t.Add(byte.Parse(((byte)((t[i - 1] << 1) ^ (0x1b))).ToString()));
                }
                else
                {
                    t.Add(byte.Parse(((byte)(t[i - 1] << 1)).ToString()));
                }
            }
            return t;
        }
        public List<List<Byte>> IMix_Col(List<List<Byte>> temp)
        {



            byte[,] Mat2 = new byte[4, 4] { {0X0E,0X0B, 0X0D,0X09},
                                            { 0X09,0X0E,0X0B,0X0D},
                                            { 0X0D,0X09,0X0E,0X0B},
                                            { 0X0B,0X0D,0X09,0X0E}};

            List<List<Byte>> ret = new List<List<Byte>>();
            for (int isa = 0; isa < 4; isa++)
            {
                List<Byte> row = new List<Byte>();
                for (int j = 0; j < 4; j++)
                {
                    row.Add(0x00);
                }
                ret.Add(row);
            }

            for (int j = 0; j < temp.Count; j++)
            {

                for (int k = 0; k < 4; k++)
                {

                    byte outs = 0x00;
                    for (int l = 0; l < 4; l++)
                    {
                        List<byte> The_Byte_0 = MM(temp[l][k]);

                        byte t = Mat2[j, l];
                        List<bool> bitarray = new List<bool>();

                        for (int isa = 7; isa >= 0; isa--)
                        {

                            bool bit = ((t >> isa) & 1) == 1;
                            bitarray.Add(bit);
                        }
                        bitarray.Reverse();

                        for (int kk = 0; kk < 4; kk++)
                        {
                            if (bitarray[kk] == true)
                            {
                                outs = (byte)(outs ^ The_Byte_0[kk]);
                            }
                        }
                    }
                    ret[j][k] = outs;
                }
            }

            return ret;
        }

        public List<List<Byte>> Irotate(List<List<Byte>> Temp)
        {
            for (int j = 1; j < Temp.Count; j++)
            {
                if (j == 1)
                {
                    byte temp1 = Temp[j][3];
                    Temp[j][3] = Temp[j][2];
                    Temp[j][2] = Temp[j][1];
                    Temp[j][1] = Temp[j][0];
                    Temp[j][0] = temp1;
                }
                else if (j == 2)
                {
                    byte temp1 = Temp[j][3];
                    byte temp2 = Temp[j][2];
                    Temp[j][2] = Temp[j][0];
                    Temp[j][3] = Temp[j][1];
                    Temp[j][1] = temp1;
                    Temp[j][0] = temp2;
                }
                else if (j == 3)
                {
                    byte temp1 = Temp[j][0];
                    byte temp2 = Temp[j][1];
                    byte temp3 = Temp[j][2];
                    Temp[j][2] = Temp[j][3];
                    Temp[j][3] = temp1;
                    Temp[j][0] = temp2;
                    Temp[j][1] = temp3;
                }
            }

            return Temp;
        }

        public List<List<Byte>> ISB(List<List<Byte>> temp)
        {


            for (int j = 0; j < temp.Count; j++)
            {
                for (int k = 0; k < temp[j].Count; k++)
                {


                    temp[j][k] = I_SB[temp[j][k] >> 4, temp[j][k] & 0x0f];
                }
            }


            return temp;
        }

        public List<List<byte>> split_text(string text)
        {
            List<List<byte>> val = new List<List<byte>>();
            if (text.Substring(0, 2) == "0x")
            {
                text = text.Substring(2, text.Length - 2);

            }

            for (int j = 0; j < 4; j++)
            {
                List<byte> temp = new List<byte>();
                for (int k = 0; k < 4; k++)
                {
                    temp.Add(0x00);
                }
                val.Add(temp);
            }
            int i = 0;
            for (int j = 0; j < 4; j++)
            {
                List<byte> temp = new List<byte>();
                for (int k = 0; k < 4; k++)
                {


                    val[k][j] = Convert.ToByte(text.Substring(i, 2), 16);
                    i += 2;
                }

            }

            return val;

        }
        public List<List<Byte>> AddRoun(List<List<Byte>> temp, List<List<Byte>> key)
        {

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    temp[i][j] = (Byte)((int)temp[i][j] ^ (int)key[i][j]);
            return temp;
        }
        public List<List<Byte>> SB(List<List<Byte>> temp)
        {


            for (int j = 0; j < temp.Count; j++)
            {
                for (int k = 0; k < temp[j].Count; k++)
                {


                    temp[j][k] = s_SB[temp[j][k] >> 4, temp[j][k] & 0x0f];
                }
            }


            return temp;
        }
        public List<List<Byte>> rotate(List<List<Byte>> Temp)
        {
            for (int j = 1; j < Temp.Count; j++)
            {
                if (j == 1)
                {
                    byte temp1 = Temp[j][0];
                    Temp[j][0] = Temp[j][1];
                    Temp[j][1] = Temp[j][2];
                    Temp[j][2] = Temp[j][3];
                    Temp[j][3] = temp1;
                }
                else if (j == 2)
                {
                    byte temp1 = Temp[j][0];
                    byte temp2 = Temp[j][1];
                    Temp[j][0] = Temp[j][2];
                    Temp[j][1] = Temp[j][3];
                    Temp[j][2] = temp1;
                    Temp[j][3] = temp2;
                }
                else if (j == 3)
                {
                    byte temp1 = Temp[j][0];
                    byte temp2 = Temp[j][1];
                    byte temp3 = Temp[j][2];
                    Temp[j][0] = Temp[j][3];
                    Temp[j][1] = temp1;
                    Temp[j][2] = temp2;
                    Temp[j][3] = temp3;
                }
            }

            return Temp;
        }

        public List<List<Byte>> Keysec(int i, List<List<Byte>> temp)
        {
            List<List<Byte>> val = new List<List<byte>>();
            List<byte> Rac = new List<byte>();
            for (int k = 0; k < 4; k++)
            {

                Rac.Add(R_mat[k, i]);

            }


            List<byte> key = new List<byte>();


            for (int jj = 1; jj < temp.Count; jj++)
            {
                key.Add(temp[jj][3]);
            }
            key.Add(temp[0][3]);


            for (int k = 0; k < key.Count; k++)
            {


                key[k] = (byte)((int)(s_SB[key[k] >> 4, key[k] & 0x0f]) ^ (int)Rac[k]);
            }

            for (int jj = 0; jj < temp.Count; jj++)
            {
                for (int jjj = 0; jjj < temp[jj].Count; jjj++)
                {
                    temp[jjj][jj] = (byte)((int)key[jjj] ^ (int)temp[jjj][jj]);
                    key[jjj] = temp[jjj][jj];
                }
            }


            return temp;

        }
        public List<List<Byte>> Mix_Col(List<List<Byte>> temp)
        {
            byte[,] Mat1 = new byte[4, 4] {{0x02,0x03,0x01,0x01},
                                           {0x01,0x02,0x03,0x01},
                                           {0x01,0x01,0x02,0x03},
                                           {0x03,0x01,0x01,0x02}};
            List<List<Byte>> ret = new List<List<Byte>>();


            for (int i = 0; i < 4; i++)
            {
                List<Byte> row = new List<Byte>();
                for (int j = 0; j < 4; j++)
                {
                    row.Add(0x00);
                }
                ret.Add(row);
            }
            for (int j = 0; j < temp.Count; j++)
            {

                for (int k = 0; k < 4; k++)
                {
                    byte outs = 0x00;

                    for (int l = 0; l < 4; l++)
                    {
                        byte temp2;

                        temp2 = temp[l][j];
                        if (Mat1[k, l] == 0x02)
                        {


                            temp2 = (byte)(temp[l][j] << 1);
                            byte temp7 = (byte)(temp[l][j] & 0x80);
                            if (temp7 == 0x80)
                            {
                                temp2 = (byte)(temp2 ^ 0x1b);
                            }
                        }
                        else if (Mat1[k, l] == (byte)0x03)
                        {
                            temp2 = (byte)(temp[l][j] << 1);
                            byte temp7 = (byte)(temp[l][j] & 0x80);
                            if (temp7 == 0x80)
                            {
                                temp2 = (byte)(temp2 ^ 0x1b);
                            }
                            temp2 = (byte)(temp[l][j] ^ temp2);

                        }
                        else if (Mat1[k, l] == (byte)0x01)
                        {
                            temp2 = temp[l][j];
                        }
                        outs = (byte)(outs ^ temp2);
                    }
                    ret[k][j] = outs;
                }


            }

            return ret;
        }


        public override string Encrypt(string plainText, string key)
        {

            List<List<byte>> text_val = split_text(plainText);
            List<List<byte>> keys = split_text(key);
            text_val = AddRoun(text_val, keys);

            for (int i = 0; i < 9; i++)
            {
                text_val = SB(text_val);
                text_val = rotate(text_val);
                text_val = Mix_Col(text_val);
                keys = Keysec(i, keys);
                text_val = AddRoun(text_val, keys);
            }

            text_val = SB(text_val);
            text_val = rotate(text_val);
            keys = Keysec(9, keys);
            text_val = AddRoun(text_val, keys);

            return B_to_S(text_val);
        }


        private string B_to_S(List<List<Byte>> temp)
        {
            string outs = "0x";
            for (int h = 0; h < 4; h++)
            {
                for (int hh = 0; hh < 4; hh++)
                {

                    string res = Convert.ToString(temp[hh][h], 16);
                    if (res.Length == 1)
                    {
                        res = "0" + res;
                    }
                    outs += res;

                }
            }
            return outs;
        }


    }
}
