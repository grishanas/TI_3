using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace TI_3
{
    class RSA
    {
        static char[] alpha = new char[]
        {
            ' ','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','P','Q','R','S','T','U','V','W','X','Y','Z'
        };
        const int H0 = 100;
        const int MINVALUE= 100;
        const int MAXVALUE= 10000;

        private BigInteger q;
        private BigInteger p;
        private BigInteger m;
        private BigInteger n;
        private BigInteger d;
        private BigInteger e;

        public BigInteger[] PrivateKey;
        public BigInteger[] Publickey;


        public RSA()
        {
            PrivateKey = new BigInteger[2];
            Publickey = new BigInteger[2];

        }
        private bool Is_simple(BigInteger a)
        {
            if (a < 2)
                return false;
            if (a == 2)
                return true;
            for (int i = 2; i < (int)(a / 2); i++)
            {
                if (a % i == 0)
                    return false;
            }
            return true;
        }
        public void CreateKey(int Q = 0,int P=0)
        {
            if(Q!=0&&P!=0)
            {
                q = Q;
                p = P;
            }
            else
            {
                bool IsSimple = false;
                var random = new Random();
                while(!IsSimple)
                {
                    IsSimple = true;
                    q = random.Next(MINVALUE, MAXVALUE);
                    IsSimple = Is_simple(q);
                }
                IsSimple = false;
                while (!IsSimple)
                {
                    IsSimple = true;
                    p = random.Next(MINVALUE, MAXVALUE);
                    IsSimple = Is_simple(p);
                }

            }

            BigInteger n = p * q;
            BigInteger m = (p - 1) * (q - 1);



            BigInteger d = Calculate_D(m); ;
            BigInteger e = Calculate_E(d, m);
            Publickey[0] = d;
            Publickey[1] = n;
            PrivateKey[0] = e;
            PrivateKey[1] = n;
        }


        private BigInteger Calculate_E(BigInteger d, BigInteger m)
        {

            BigInteger e = 3;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }

        private BigInteger Calculate_D(BigInteger m)
        {
            BigInteger d = 3;

            for (BigInteger i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0))
                {
                    d++;
                    i = 1;
                }

            return d;
        }

        private BigInteger GenerateHash(string Message)
        {
            BigInteger H = H0;
            for(int i=0;i<Message.Length;i++)
            {
                H= fastexp(H+Array.IndexOf(alpha,Message[i]),2,n);
            }
            return H;
        }

        public BigInteger CreateSig(string Message)
        {
            n = PrivateKey[1];
            return fastexp(GenerateHash(Message), PrivateKey[0], PrivateKey[1]);
        }

        private BigInteger fastexp(BigInteger a, BigInteger z, BigInteger N)
        {
            BigInteger x = 1;

            for (; !z.IsZero; --z)
            {
                while (z.IsEven)
                {
                    z /= 2;

                    a = (a * a) % N;
                }

                x = (x * a) % N;
            }

            return x;
        }

        public bool ChekSig(BigInteger Code, string Message)
        {
            n = Publickey[1];
            if (GenerateHash(Message) == fastexp(Code, Publickey[0],Publickey[1]))
                return true;
            return false;

            
        }
    }
}
