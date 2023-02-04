using Syncfusion.Maui.Core.Hosting;

namespace PdfToolBoxMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSentry(options =>
			{
				// TODO: Set the Sentry Dsn
				options.Dsn = "https://examplePublicKey@o0.ingest.sentry.io/0";
			})
			.UseMauiCommunityToolkit()
			.ConfigureSyncfusionCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
				fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
				fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<FileHelperService>();
		builder.Services.AddSingleton<SampleViewModel>();

		builder.Services.AddSingleton<SamplePage>();

		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddSingleton<WebViewViewModel>();

		builder.Services.AddSingleton<WebViewPage>();

		builder.Services.AddTransient<SampleDataService>();
		builder.Services.AddTransient<PdfListDetailViewModel>();
		builder.Services.AddTransient<PdfListDetailPage>();

		builder.Services.AddSingleton<PdfListViewModel>();

		builder.Services.AddSingleton<PdfListPage>();

		builder.Services.AddSingleton<SplitPdfViewModel>();

		builder.Services.AddSingleton<SplitPdfPage>();

		builder.Services.AddSingleton<MergePdfViewModel>();

		builder.Services.AddSingleton<MergePdfPage>();

		builder.Services.AddSingleton<ProtectPdfViewModel>();

		builder.Services.AddSingleton<ProtectPdfPage>();

		builder.Services.AddSingleton<WatermarkPdfViewModel>();

		builder.Services.AddSingleton<WatermarkPdfPage>();

		builder.Services.AddSingleton<CompressPdfViewModel>();

		builder.Services.AddSingleton<CompressPdfPage>();

		builder.Services.AddSingleton<ToImagePdfViewModel>();

		builder.Services.AddSingleton<ToImagePdfPage>();

		builder.Services.AddSingleton<EditPagePdfViewModel>();

		builder.Services.AddSingleton<EditPagePdfPage>();

		builder.Services.AddSingleton<AboutViewModel>();

		builder.Services.AddSingleton<AboutPage>();

		builder.Services.AddSingleton(AudioManager.Current);

		return builder.Build();
	}
}
