using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App2.Droid
{
    class ManualVerificationProvider : IXjtuCardPasswordProvider, IVerificationCodeRecognizer
    {
        public ManualVerificationProvider(Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
        }

        public Context Context { get; }

        public async Task<string> GetPasswordAsync(Stream keypadImageStream, SiteManager site)
        {
            return "000000";
        }

        public Task<string> RecognizeAsync(Stream imageStream, SiteManager site)
        {
            var tcs = new TaskCompletionSource<string>();
            var adb = new AlertDialog.Builder(Context);
            using (var inflater = LayoutInflater.From(adb.Context))
            {
                var view = inflater.Inflate(Resource.Layout.VerificationCodeInput, null);
                var imageView = view.FindViewById<ImageView>(Resource.Id.verificationImageView);
                var editText = view.FindViewById<EditText>(Resource.Id.verificationEditText);
                imageView.SetImageBitmap(BitmapFactory.DecodeStream(imageStream));
                adb.SetView(view)
                    .SetPositiveButton("确定", (_, e) =>
                    {
                        tcs.SetResult(editText.Text);
                    })
                    .SetNegativeButton("取消", (_, e) =>
                    {
                        tcs.SetResult(null);
                    })
                    .Show();
            }
            return tcs.Task;
        }
    }
}