using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using System.IO;

namespace DCC_Standard
{

	public class DCC_PDF : Document
	{

		public PdfPTable Table;
		public PdfPCell CellLabel, CellText, CellBlank;
		public Paragraph Paragraph;
		public MemoryStream ms;
		public BaseFont bf;
		public iTextSharp.text.Font fontTitle, fontHeading, fontNormal, fontBold, fontItalic, fontSmall, fontSmallItalic, fontTiny;


		public DCC_PDF()
		{
			fontTitle = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD);
			fontHeading = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
			fontNormal = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
			fontBold = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
			fontItalic = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC);
			fontSmall = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);
			fontSmallItalic = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.ITALIC);
			fontTiny = FontFactory.GetFont("Arial", 4, iTextSharp.text.Font.NORMAL);
		}


		public void Start(bool isLandscape = false)
		{
			SetMargins(36, 36, 36, 36); //L,R,T,B - 36pt = ½ inch; 72pt = 1 inch
			if (isLandscape)
			{
				//SetPageSize(iTextSharp.text.PageSize.A4);  //A4.Rotate() for landscape
				SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
			}
			ms = new MemoryStream();
			PdfWriter.GetInstance(this, ms).CloseStream = false;
			Open();
		}


		public MemoryStream Finish()
		{
			Close();
			byte[] byteInfo = ms.ToArray();
			ms.Write(byteInfo, 0, byteInfo.Length);
			ms.Position = 0;
			return ms;
		}


		public void AddTitle(string text, bool firstPage = false)
		{
			if (!firstPage)
				NewPage();
			Paragraph = new Paragraph(text, fontTitle);
			Paragraph.Alignment = Element.ALIGN_CENTER;
			Paragraph.SpacingAfter = 20;
			Add(Paragraph);
			return;
		}


		public void AddParagraph(string text = null, string heading = null)
		{
			if (heading != null)
			{
				Paragraph = new Paragraph(heading, fontHeading);
				Paragraph.Alignment = Element.ALIGN_LEFT;
				Paragraph.SpacingAfter = 5;
				Add(Paragraph);
			}

			if (text != null)
			{
				Paragraph = new Paragraph(text, fontNormal);
				Paragraph.Alignment = Element.ALIGN_LEFT;
				Paragraph.SpacingAfter = 10;
				Add(Paragraph);
			}

			return;
		}


		public void TableStart(int NoColumns, float[] widths)
		{
			if (widths.Length != NoColumns)
				throw new System.ArgumentException("No of Columns doesn't equal no of Widths");

			Table = new PdfPTable(NoColumns);
			Table.TotalWidth = 800;
			Table.WidthPercentage = 100;
			// GWW new 2 lines replacing totalwidth=530f
			Table.SetWidths(widths);
			Table.LockedWidth = true;
			Table.SpacingBefore = 5;
			Table.SpacingAfter = 5;
		}


		public void TableFinish()
		{
			Add(Table);
		}


		public void AddTableTitle(string text)
		{
			CellBlank = new PdfPCell(new Phrase(" ", fontTiny));
			CellBlank.Colspan = Table.NumberOfColumns;
			CellBlank.BorderColor = BaseColor.WHITE;

			CellText = new PdfPCell(new Phrase(text, fontHeading));
			CellText.Colspan = Table.NumberOfColumns;
			CellText.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
			CellText.BorderColor = BaseColor.WHITE;
			Table.AddCell(CellBlank);
			Table.AddCell(CellBlank);
			Table.AddCell(CellBlank);
			Table.AddCell(CellText);
			Table.AddCell(CellBlank);
			return;
		}


		public void AddTableHeading(string text)
		{
			CellBlank = new PdfPCell(new Phrase(" ", fontTiny));
			CellBlank.Colspan = Table.NumberOfColumns;
			CellBlank.BorderColor = BaseColor.WHITE;

			CellText = new PdfPCell(new Phrase(text, fontNormal));
			CellText.Colspan = Table.NumberOfColumns;
			CellText.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
			CellText.BorderColor = BaseColor.WHITE;
			Table.AddCell(CellBlank);
			Table.AddCell(CellBlank);
			Table.AddCell(CellText);
			Table.AddCell(CellBlank);
			return;
		}


		public void AddTableRow(string[] texts, int[] rotates = null, int[] align = null, bool whiteBackground = false, bool whiteBorder = false, bool smallerFont = false, System.Single headerHeight = 60f, iTextSharp.text.Font font = null)
		{
			if (texts.Length != Table.NumberOfColumns)
				throw new System.ArgumentException("No of Columns doesn't equal no of Labels & Texts");

			bool rotated = false;
			if (rotates == null)
			{
				rotated = true;
				int[] r = new int[Table.NumberOfColumns];
				for (int i = 0; i < Table.NumberOfColumns; i++)
					r[i] = 0;
				rotates = r;
			}

			if (font != null)
			{
			}

			BaseColor backgroundColour = new BaseColor(ColorTranslator.FromHtml("#FFE0F6FD"));
			if (whiteBackground)
				backgroundColour = BaseColor.WHITE;

			BaseColor borderColour;
			if (whiteBorder)
				borderColour = BaseColor.WHITE;
			else
				borderColour = BaseColor.GRAY;

			CellBlank = new PdfPCell(new Phrase(" ", fontTiny));
			CellBlank.Colspan = Table.NumberOfColumns;
			CellBlank.BorderColor = borderColour;

			if (string.IsNullOrEmpty(texts[0]))
			{
				Table.AddCell(CellBlank);
			}
			for (int i = 0; i < Table.NumberOfColumns; i++)
			{
				CellText = new PdfPCell(new Phrase(texts[i], ((smallerFont) ? fontSmall : fontNormal)));
				if (i == 0)
				{
					CellText.BorderColor = BaseColor.WHITE;
				}
				else
				{
					if (texts[0] == " ") // if it's a header row
					//if (string.IsNullOrEmpty(texts[0])) //////////// gww 2019
					{
						if (texts[i] == "")
						{
							CellText.BorderColor = BaseColor.WHITE;
						}
						else
						{
							CellText.BorderColor = borderColour;
							CellText.Rotation = rotates[i];
							CellText.BackgroundColor = BaseColor.WHITE;/////////////////////////gww 2019 backgroundColour;
							CellText.FixedHeight = headerHeight;
							if (rotated)
								CellText.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
							else if (align != null)
								CellText.HorizontalAlignment = align[i];
							CellText.VerticalAlignment = PdfPCell.ALIGN_TOP;////////////gww 2019 changed from bottom
						}
					}
					else // if it's NOT a header row
					{
						if (texts[i] == "-")
						{
							CellText = new PdfPCell(new Phrase(texts[i], ((smallerFont) ? fontSmall : fontNormal)));
							CellText.BorderColor = BaseColor.WHITE;////////////////////////// BaseColor.LIGHT_GRAY;
							if (align != null)
								CellText.HorizontalAlignment = align[i];
							else
								CellText.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
							//CellText.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
						}
						else if (texts[i] == "")
						{
							CellText.BorderColor = BaseColor.WHITE;
						}
						else
						{
							CellText = new PdfPCell(new Phrase(texts[i], ((smallerFont) ? fontSmall : fontNormal)));
							//CellText.BorderColor = borderColour; ////////////////////////// BaseColor.LIGHT_GRAY;   gww 2019
							CellText.BorderColor = BaseColor.WHITE;
							if (align != null)
								CellText.HorizontalAlignment = align[i];
							else
								CellText.HorizontalAlignment = PdfPCell.ALIGN_LEFT;////////////////////////////gww 2019 was right
							//CellText.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
						}
					}
				}
				CellText.Padding = 2f;/////////////row margins
				CellText.UseAscender = true;
				CellText.UseDescender = true;
				Table.AddCell(CellText);
			}
			if (string.IsNullOrEmpty(texts[0]))
			{
				Table.AddCell(CellBlank);
			}

			return;
		}


		public void AddTableBlankRow(bool whiteBorder = false)
		{
			if (whiteBorder)
				CellBlank.BorderColor = BaseColor.WHITE;

			Table.AddCell(CellBlank);
			Table.AddCell(CellBlank);
			return;
		}


		public static PdfPCell Cell(string pContent, iTextSharp.text.Font pFont = null, int pColSpan = 1, int pBorder = 0, int pHorizontalAlignment = 0)
		{
			if (pFont == null)
				pFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
			PdfPCell cell = new PdfPCell(new Phrase(pContent, pFont));
			cell.Border = pBorder;
			cell.Colspan = pColSpan;
			cell.HorizontalAlignment = pHorizontalAlignment;
			return cell;
		}


		public static PdfPCell HeaderCell(string pContent, int pColSpan = 1, int pHorizontalAlignment = 0)
		{
			PdfPCell cell = new PdfPCell(new Phrase(pContent, FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD)));
			cell.Colspan = pColSpan;
			cell.HorizontalAlignment = pHorizontalAlignment;
			cell.BackgroundColor = BaseColor.LIGHT_GRAY;
			return cell;
		}

	}

}
