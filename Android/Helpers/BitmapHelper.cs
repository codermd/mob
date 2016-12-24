using System;
using System.Threading.Tasks;
using Android.Graphics;
using System.IO;
using Android.Util;
using Android.Media;
using Android.Content;

namespace Mxp.Droid
{
	public static class BitmapHelper {
		public static async Task<Bitmap> ResizeBitmapAsync (System.IO.Stream input, int maxImageSize) {
			BitmapFactory.Options bmOptions = new BitmapFactory.Options {
				InJustDecodeBounds = false
			};

			using (Bitmap bitmap = await BitmapFactory.DecodeStreamAsync (input, null, bmOptions)) {
				return CreateScaleBitmap (bitmap, maxImageSize);
			}
		}

		public static async Task<Bitmap> ResizeBitmapAsync (string filePath, int maxImageSize) {
			BitmapFactory.Options bmOptions = new BitmapFactory.Options {
				InJustDecodeBounds = false
			};
			Bitmap bitmap = await BitmapFactory.DecodeFileAsync (filePath, bmOptions);

			return CreateScaleBitmap (bitmap, maxImageSize);
		}

		private static Bitmap CreateScaleBitmap (Bitmap bitmap, int maxImageSize) {
			double ratio = Math.Min ((double)maxImageSize / bitmap.Width, (double)maxImageSize / bitmap.Height);
			int width = (int)Math.Round ((double)ratio * bitmap.Width);
			int height = (int)Math.Round ((double)ratio * bitmap.Height);

			return Bitmap.CreateScaledBitmap (bitmap, width, height, true);
		}

		// https://developer.android.com/training/displaying-bitmaps/load-bitmap.html
		public static async Task<Bitmap> ResizeBitmapInSampleSizeAsync (string filePath, int maxImageSize) {
			BitmapFactory.Options bmOptions = new BitmapFactory.Options {
				InJustDecodeBounds = true
			};

			bmOptions.InSampleSize = CalculateInSampleSize (bmOptions, maxImageSize);

			bmOptions.InJustDecodeBounds = false;

			return await BitmapFactory.DecodeFileAsync (filePath, bmOptions);
		}

		public static async Task<Bitmap> ResizeAndRotateBitmapAsync (string filePath, int maxImageSize) {
			BitmapFactory.Options bmOptions = new BitmapFactory.Options {
				InJustDecodeBounds = false
			};
			Bitmap bitmap = await BitmapFactory.DecodeFileAsync (filePath, bmOptions);

			double ratio = Math.Min ((double)maxImageSize / bitmap.Width, (double)maxImageSize / bitmap.Height);

			Matrix matrix = new Matrix ();
			matrix.PostScale ((float)ratio, (float)ratio);
			matrix.PostRotate (GetOrientation (filePath));

			return Bitmap.CreateBitmap (bitmap, 0, 0, bmOptions.OutWidth, bmOptions.OutHeight, matrix, true);
		}

		public static async Task<Bitmap> ResizeAndRotateBitmapAsync (Android.Net.Uri imageUri, Context context, int maxImageSize) {
			BitmapFactory.Options bmOptions = new BitmapFactory.Options {
				InJustDecodeBounds = false
			};

			using (System.IO.Stream stream = context.ContentResolver.OpenInputStream (imageUri)) {
				Bitmap bitmap = await BitmapFactory.DecodeStreamAsync (stream, null, bmOptions);

				double ratio = Math.Min ((double)maxImageSize / bitmap.Width, (double)maxImageSize / bitmap.Height);

				Matrix matrix = new Matrix ();
				matrix.PostScale ((float)ratio, (float)ratio);
				matrix.PostRotate (GetOrientation (imageUri.GetPath (context)));

				return Bitmap.CreateBitmap (bitmap, 0, 0, bmOptions.OutWidth, bmOptions.OutHeight, matrix, true);
			}
		}

		public static async Task<Bitmap> RotateBitmapAsync (string filePath) {
			BitmapFactory.Options bmOptions = new BitmapFactory.Options {
				InJustDecodeBounds = false
			};
			Bitmap bitmap = await BitmapFactory.DecodeFileAsync (filePath, bmOptions);

			Matrix matrix = new Matrix ();
			matrix.SetRotate (GetOrientation (filePath));

			return Bitmap.CreateBitmap (bitmap, 0, 0, bmOptions.OutWidth, bmOptions.OutHeight, matrix, true);
		}

		private static int GetOrientation (string filePath) {
			ExifInterface exif = null;

			try {
				exif = new ExifInterface (filePath);
			} catch (Java.IO.IOException) {
				return default (int);
			}

			Orientation orientation = (Orientation)exif.GetAttributeInt (ExifInterface.TagOrientation, (int)Orientation.Normal);

			switch (orientation) {
				case Orientation.Rotate90:
					return 90;
				case Orientation.Rotate180:
					return 180;
				case Orientation.Rotate270:
					return 270;
				default:
					return 0;
			}
		}

		/*
		 * A power of two value is calculated because the decoder uses a final value by rounding down
		 * to the nearest power of two, as per the inSampleSize documentation.
		 * Source : http://developer.android.com/training/displaying-bitmaps/load-bitmap.html
		*/
		private static int CalculateInSampleSize (BitmapFactory.Options options, int maxImageSize) {
			int height = options.OutHeight;
			int width = options.OutWidth;
			int inSampleSize = 1;

			if (height > maxImageSize || width > maxImageSize) {
				int halfHeight = height / 2;
				int halfWidth = width / 2;

				// Calculate the largest inSampleSize value that is a power of 2 and keeps both
				// height and width larger than the requested height and width.
				while ((halfHeight / inSampleSize) > maxImageSize
					&& (halfWidth / inSampleSize) > maxImageSize) {
					inSampleSize *= 2;
				}
			}

			return inSampleSize;
		}

		public static async Task<string> EncodeToBase64 (Bitmap bitmap) {
			using (MemoryStream memoryStream = new MemoryStream (bitmap.ByteCount)) {
				// Alternative : http://stackoverflow.com/questions/4830711/how-to-convert-a-image-into-base64-string/17874349
				await bitmap.CompressAsync (Bitmap.CompressFormat.Jpeg, 60, memoryStream);

				bitmap.Recycle ();

				byte [] byteArray = memoryStream.ToArray ();

				return EncodeToString (byteArray);
			}
		}

		public static async Task<Bitmap> DecodeBase64 (string base64) {
			byte [] decodedBytes = GetBytes (base64);
			return await DecodeBytes (decodedBytes);
		}

		public static async Task<Bitmap> DecodeBytes (byte [] decodedBytes) {
			return await BitmapFactory.DecodeByteArrayAsync (decodedBytes, 0, decodedBytes.Length);
		}

		public static byte [] GetBytes (string base64) {
			return Base64.Decode (base64, Base64Flags.Default);
		}

		public static string EncodeToString (byte [] bytes) {
			return Base64.EncodeToString (bytes, Base64Flags.NoWrap);
		}
	}
}
