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

        public Task<string> GetPasswordAsync(Stream keypadImageStream, IList<MapAreaInfo> mapAreas, SiteManager site)
        {
            var tcs = new TaskCompletionSource<string>();
            var adb = new AlertDialog.Builder(Context);
            using (var inflater = LayoutInflater.From(adb.Context))
            {
                var view = inflater.Inflate(Resource.Layout.XjtuCardPassword, null);
                var passwordView = view.FindViewById<TextView>(Resource.Id.passwordTextView);
                var padTable = view.FindViewById<TableLayout>(Resource.Id.passwordPadTable);
                var currentPassword = "";
                Action updatePasswordDisplay = () =>
                {
                    passwordView.Text = new string('#', currentPassword.Length);
                };
                //生成按键。
                var keypadBitmap = BitmapFactory.DecodeStream(keypadImageStream);
                for (var row = 0; row < 4; row++)
                {
                    var tr = new TableRow(adb.Context)
                    {
                        LayoutParameters = new ViewGroup.LayoutParams(
                            ViewGroup.LayoutParams.WrapContent,
                            ViewGroup.LayoutParams.WrapContent)
                    };
                    padTable.AddView(tr);
                    for (var col = 0; col < 3; col++)
                    {
                        var index = row * 3 + col;
                        var indexExpr = Convert.ToString(index);
                        View buttonView = null;
                        if (index <= 9)
                        {
                            var area = mapAreas.First(a => a.Value == indexExpr);
                            var button = new ImageButton(adb.Context)
                            {
                                LayoutParameters = new TableRow.LayoutParams(
                                    ViewGroup.LayoutParams.WrapContent,
                                    ViewGroup.LayoutParams.WrapContent),
                            };
                            button.SetMinimumWidth(DroidUtility.DipToPixelsX(64));
                            button.SetMinimumHeight(DroidUtility.DipToPixelsY(64));
                            button.SetImageBitmap(Bitmap.CreateBitmap(keypadBitmap, area.X1, area.Y1, area.Width, area.Height));
                            button.SetScaleType(ImageView.ScaleType.FitCenter);
                            button.Click += (_, e) =>
                            {
                                currentPassword += indexExpr;
                                updatePasswordDisplay();
                            };
                            buttonView = button;
                        } else if (index == 10)
                        {
                            var button = new Button(adb.Context)
                            {
                                Text = "更正",
                                LayoutParameters = new TableRow.LayoutParams(
                                    ViewGroup.LayoutParams.MatchParent,
                                    ViewGroup.LayoutParams.WrapContent)
                                {
                                    Span = 2,
                                    Gravity = GravityFlags.CenterVertical
                                }
                            };
                            button.Click += (_, e) =>
                            {
                                currentPassword = "";
                                updatePasswordDisplay();
                            };
                            buttonView = button;
                        }
                        if (buttonView != null)
                        {
                            tr.AddView(buttonView);
                        }
                    }
                }
                //初始化界面。
                updatePasswordDisplay();
                adb.SetView(view)
                    .SetPositiveButton("确定", (_, e) =>
                    {
                        tcs.SetResult(currentPassword);
                    })
                    .SetNegativeButton("取消", (_, e) =>
                    {
                        tcs.SetResult(null);
                    })
                    .Show();
            }
            return tcs.Task;
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
                editText.RequestFocus();
            }
            return tcs.Task;
        }
    }
}