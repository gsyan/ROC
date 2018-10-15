using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using _uint64 = System.UInt64;


namespace AppSecurity
{
    static class Constants
    {
		public const int STRG_COL_NUM = 15;
		public const int STRG_LOW_NUM = 20;
		public const int INVALID_INDEX = -1;
		public const int CHUNK_NUM = 5;         
    }


    public class WellRng
    {   
		private const int WELLRNG512_TB_NUM = 16;
		private static uint _index = 0;
		
		private static uint[] _state = new uint[WELLRNG512_TB_NUM]
		{
			0x18b74777, 0x88085ae6, 0xff0f6a70, 0x66063bca,
			0x8f659eff, 0xf862ae69, 0x616bffd3, 0x166ccf45,
			0xd70dd2ee, 0x4e048354, 0x3903b3c2, 0xa7672661,
			0x4969474d, 0x3e6e77db, 0xaed16a4a, 0xd9d65adc
		};
		
		
		public WellRng()
		{    
		}
		
		
		public static void WELLINIT()
		{
			Random r = new Random();
			_index = (uint)r.Next(0, WELLRNG512_TB_NUM);
		}
		

		public static uint WELLRNG512()
		{
			uint a, b, c, d;
			
			a = _state[_index];
			c = _state[(_index + 13) & 15];
			b = a ^ c ^ (a << 16) ^ (c << 15);
			c = _state[(_index + 9) & 15];
			c ^= (c >> 11);
			a = _state[_index] = b ^ c;
			d = a ^ ((a << 5) & 0xDA442D20);
			_index = (_index + 15) & 15;
			a = _state[_index];
			_state[_index] = a ^ b ^ d ^ (a << 2) ^ (b << 18) ^ (c << 28);
			
			return _state[_index];
		}
    }


    public struct DATA_BITSTRG
    {
		public _uint64[] activeChunk;
		public bool isActive;
    };

    
    public class SecureVarBase
    {
		public int _dataType;
		
		protected int _searchIndex = 0;
		protected int _oldRow = 0;
		protected int _shiftTimeSec;
		protected int _lastClock;
		protected bool _isInit = true;
		
		protected const int	_shiftTimeMin = 700;
		protected const int	_shiftTimeMax = 1500;
		
		protected DATA_BITSTRG[,] _bitStrg;
		protected WellRng _wellRng;
		
		
		public SecureVarBase()
		{
		    Initialize();
		}
		
		
		protected void Initialize()
		{
			WellRng.WELLINIT();
			
			_shiftTimeSec = GetRandom( _shiftTimeMin, _shiftTimeMax );
			_lastClock = GetTickTime( );
			
			AllocMem( );
		}
		
		
		protected int GetRandom(int min, int max)
		{
		    return (int)(WellRng.WELLRNG512() % (max - min + 1) + min);
		}
		
		
		protected int GetTickTime()
		{
		    return Environment.TickCount; 
		}
		
		
		protected bool AllocMem()
		{
			_bitStrg = new DATA_BITSTRG[Constants.STRG_LOW_NUM, Constants.STRG_COL_NUM];
			
			for (int i = 0; i < Constants.STRG_LOW_NUM; ++i)
			{
				for (int k = 0; k < Constants.STRG_COL_NUM; ++k)
				{
					_bitStrg[i, k].activeChunk = new _uint64[Constants.CHUNK_NUM];
				}
			}
			
			return true;
		}
		
		
		protected void GetStrgIndexToSave(out int pi, out int pk, out int pk_old)
		{
			pi = Constants.INVALID_INDEX; pk = Constants.INVALID_INDEX; pk_old = Constants.INVALID_INDEX;
			
			int i=0, k=0;
			
			if (_isInit)
			{
			    i = 0;
			    _isInit = false;
			}
			else
			{
			    int tCurrentTime = GetTickTime();
			
			    if (tCurrentTime - _lastClock >= _shiftTimeSec)
				{
			        _lastClock = tCurrentTime;
			        _shiftTimeSec = GetRandom(_shiftTimeMin, _shiftTimeMax);
			
			        ShiftCurrentStorage(out i, out k);
			    }
			    else
			    {
			        GetCurrentStrgIndex(out i, out k);
			    }
			
			    pk_old = k;
			}
			
			pi = i;
			pk = GetRandom(0, Constants.STRG_COL_NUM - 1);
		}
		
		
		protected void ShiftCurrentStorage(out int outNewRow, out int outCurColum)
		{
			int curRow = 0, newRow = 0, k = 0;
			
			GetCurrentStrgIndex(out curRow, out k);
			
			int nCount = 0;
			while (++nCount <= 5)
			{
			    newRow = GetRandom(0, Constants.STRG_LOW_NUM - 1);
				if (curRow != newRow && _oldRow != newRow)
				{
					break;
				}
			}
			
			_bitStrg[newRow, k].isActive = _bitStrg[curRow, k].isActive;
			Buffer.BlockCopy(_bitStrg[curRow, k].activeChunk, 0, _bitStrg[newRow, k].activeChunk, 0, sizeof(_uint64) * Constants.CHUNK_NUM);
			
			for (int index = 0; index < Constants.STRG_COL_NUM; ++index)
			{
			    _bitStrg[curRow, index].isActive = false;
			    Array.Clear(_bitStrg[curRow, index].activeChunk, 0, Constants.CHUNK_NUM);
			}
			
			_oldRow = curRow;
			_searchIndex = newRow;
			
			outNewRow = newRow;
			outCurColum = k;
		}
		
		
		protected void GetCurrentStrgIndex(out int pi, out int pk) 
		{
			if (_isInit)
			{
			    pi = 0; pk = 0;
			    return;
			}
			
			pi = Constants.INVALID_INDEX; pk = Constants.INVALID_INDEX;
			
			int n = _searchIndex;
			
			for (int k = 0; k < Constants.STRG_COL_NUM; ++k)
			{
			    if (_bitStrg[n, k].isActive)
			    {
			        pi = n;
			        pk = k;
			        return;
			    }
			}
			
			for (int i = 0; i < Constants.STRG_LOW_NUM; ++i)
			{
			    for (int k = 0; k < Constants.STRG_COL_NUM; ++k)
			    {
			        if (_bitStrg[i, k].isActive)
			        {
			            pi = i;
			            pk = k;
			            return;
			        }
			    }
			}
		}	    
    };


    public class SecureVar<T> : SecureVarBase where T  : IConvertible 
    {
		private _uint64[] _aw64ChunkAndBit = new _uint64[Constants.CHUNK_NUM]
		{ 
			0x00000000000000FCUL,
			0x000000000000E003UL, 
			0x0000000000FE1F00UL,
			0x0000C0FFFF010000UL,
			0xFFFF3F0000000000UL 
		};
		
		
		public SecureVar()
		{
		}
		
		
		public SecureVar(T value)
		{
		    Set(value);
		}
		
		
		public bool Set(T value)
		{
			int wTypeSize = Marshal.SizeOf(typeof(T));
			
			if (wTypeSize > sizeof(_uint64))
			{
				return false;
			}
			
			
			_uint64 w64ExtVal = 0;
			
			if (typeof(T).Equals(typeof(float)))            
			{                
			    float fValue = Convert.ToSingle((object)value);             
			    byte[] bytes = BitConverter.GetBytes(fValue);
			                 
			    uint w32Val = BitConverter.ToUInt32(bytes, 0);
			    w64ExtVal = (_uint64)w32Val;
			}
			else if (typeof(T).Equals(typeof(double)))
			{
				double dValue = Convert.ToDouble((object)value);
				byte[] bytes = BitConverter.GetBytes(dValue);
				w64ExtVal = BitConverter.ToUInt64(bytes, 0);
			}
			else
			{
				w64ExtVal = Convert.ToUInt64((object)value);
			}
			
			
			int i, k, old_k;
			
			GetStrgIndexToSave(out i, out k, out old_k);
			
			if (Constants.INVALID_INDEX != old_k)
			{
				_bitStrg[i, old_k].isActive = false;
			}
			
			for (int index = 0; index < Constants.CHUNK_NUM; ++index)
			{
			    _bitStrg[i, k].activeChunk[index] = w64ExtVal & _aw64ChunkAndBit[index];
			}
			
			_bitStrg[i, k].isActive = true;
			
			return true;
		}
		
		
		public T Get()
		{
			int i, k;
			
			GetCurrentStrgIndex(out i, out k);
			
			_uint64 w64Value = 0;
			
			for (int index = 0; index < Constants.CHUNK_NUM; ++index)
			{
			    w64Value = w64Value | _bitStrg[i, k].activeChunk[index];
			}
			
			if (typeof(T).Equals(typeof(float)))
			{
			    byte[] bytes = BitConverter.GetBytes(w64Value);
			
			    float fVal = BitConverter.ToSingle(bytes, 0);
			
			    return (T)Convert.ChangeType((object)fVal, typeof(T));
			}
			else if (typeof(T).Equals(typeof(double)))
			{
				byte[] bytes = BitConverter.GetBytes(w64Value);
			
				double dVal = BitConverter.ToDouble(bytes, 0);
			
				return (T)Convert.ChangeType((object)dVal, typeof(T));
			}
			else
			{
				return (T)Convert.ChangeType((object)w64Value, typeof(T));
			}
		}    
    };
}
