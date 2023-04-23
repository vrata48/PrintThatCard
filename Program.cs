using System.Drawing;
using System.Drawing.Imaging;
using CommandLine;
using PrintThatCard;

Parser.Default.ParseArguments<CommandLineOptions>(args)
    .WithNotParsed(errors =>
    {
        Console.WriteLine("Could not parse command line arguments:");
        
        foreach (var error in errors)
        {
            Console.WriteLine(error.ToString());
        }
    })
    .WithParsed(options =>
    {
        float pixelsPerCm = options.Dpi / 2.54f;

        float pageWidthPixels = options.PageWidth / 10f * pixelsPerCm;
        float pageHeightPixels = options.PageHeight / 10f * pixelsPerCm;
        
        using Bitmap bitmap = new Bitmap((int)pageWidthPixels, (int)pageHeightPixels);

        bitmap.SetResolution(options.Dpi, options.Dpi);

        float cardWidthPixels = options.CardWidth/10f * pixelsPerCm;
        float cardHeightPixels = options.CardHeight/10f * pixelsPerCm;

        float columnWidthPixels = pageWidthPixels / options.Columns;
        float rowHeightPixels = pageHeightPixels / options.Rows;

        SizeF cellSize = new SizeF(columnWidthPixels, rowHeightPixels);

        using var graphics = Graphics.FromImage(bitmap);
        Pen pen = new Pen(Color.Black, 1);

        Queue<string> paths = new Queue<string>();

        foreach (var path in Directory.EnumerateFiles(options.Input))
        {
            paths.Enqueue(path);
        }

        for (int column = 0; column < options.Columns; column++)
        {
            for (int row = 0; row < options.Rows; row++)
            {
                PointF leftTop = new PointF(column * columnWidthPixels, row * rowHeightPixels);

                PointF oneTop = new PointF((cellSize.Width - cardWidthPixels) / 2 + leftTop.X, 0 + leftTop.Y);
                PointF oneBottom = oneTop with { Y = oneTop.Y + cellSize.Height };

                PointF twoTop = new PointF((cellSize.Width - cardWidthPixels) / 2 + leftTop.X + cardWidthPixels, 0 + leftTop.Y);
                PointF twoBottom = twoTop with { Y = oneTop.Y + cellSize.Height };

                PointF threeTop = new PointF(0 + leftTop.X, (cellSize.Height - cardHeightPixels) / 2 + leftTop.Y);
                PointF threeBottom = threeTop with { X = threeTop.X + cellSize.Width };

                PointF fourTop = new PointF(0 + leftTop.X, (cellSize.Height - cardHeightPixels) / 2 + leftTop.Y + cardHeightPixels);
                PointF fourBottom = fourTop with { X = fourTop.X + cellSize.Width };

                if (options.Lines)
                {
                    graphics.DrawLine(pen, oneTop, oneBottom);
                    graphics.DrawLine(pen, twoTop, twoBottom);
                    graphics.DrawLine(pen, threeTop, threeBottom);
                    graphics.DrawLine(pen, fourTop, fourBottom);
                }

                if (paths.Count > 0)
                {
                    Image image = Image.FromFile(paths.Dequeue());

                    graphics.DrawImage(image, oneTop.X, threeTop.Y, cardWidthPixels, cardHeightPixels);
                }
            }
        }

        bitmap.Save(options.Output, ImageFormat.Png);
    });