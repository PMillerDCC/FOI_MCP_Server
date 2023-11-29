using System.IO;

namespace DCC_Standard
{

	public class DCC_Docx
	{

		public MemoryStream MemoryStream;


		public DCC_Docx()
		{
		}


		public void Start()
		{
			MemoryStream = new MemoryStream();
		}


		public MemoryStream Finish()
		{
			byte[] bytes = MemoryStream.ToArray();
			MemoryStream.Write(bytes, 0, bytes.Length);
			MemoryStream.Position = 0;
			return MemoryStream;
		}

	}

}
