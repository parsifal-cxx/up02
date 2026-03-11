using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LibraryAIS
{
    /// <summary>
    /// Класс для генерации CAPTCHA изображений
    /// </summary>
    public class CaptchaGenerator
    {
        private static readonly Random random = new Random();
        private static readonly string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        /// <summary>
        /// Текущий текст CAPTCHA
        /// </summary>
        public string CaptchaText { get; private set; }

        /// <summary>
        /// Генерирует новую CAPTCHA
        /// </summary>
        /// <param name="length">Количество символов (минимум 4)</param>
        /// <param name="width">Ширина изображения</param>
        /// <param name="height">Высота изображения</param>
        /// <returns>Изображение CAPTCHA</returns>
        public Bitmap GenerateCaptcha(int length = 4, int width = 200, int height = 60)
        {
            if (length < 4) length = 4;

            // Генерация случайного текста
            CaptchaText = GenerateRandomText(length);

            // Создание изображения
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);

            // Настройка качества отрисовки
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Заливка фона
            graphics.Clear(Color.White);

            // Добавление фонового шума (точки)
            DrawBackgroundNoise(graphics, width, height);

            // Отрисовка символов со смещением
            DrawCaptchaText(graphics, width, height);

            // Добавление линий-помех (перечёркивание)
            DrawNoiseLines(graphics, width, height);

            // Добавление дополнительного шума поверх
            DrawForegroundNoise(graphics, width, height);

            graphics.Dispose();
            return bitmap;
        }

        /// <summary>
        /// Генерирует случайную строку
        /// </summary>
        private string GenerateRandomText(int length)
        {
            char[] text = new char[length];
            for (int i = 0; i < length; i++)
            {
                text[i] = chars[random.Next(chars.Length)];
            }
            return new string(text);
        }

        /// <summary>
        /// Рисует фоновый шум (точки и мелкие элементы)
        /// </summary>
        private void DrawBackgroundNoise(Graphics graphics, int width, int height)
        {
            // Случайные точки
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(width);
                int y = random.Next(height);
                int size = random.Next(1, 4);
                Color color = Color.FromArgb(
                    random.Next(150, 220),
                    random.Next(150, 220),
                    random.Next(150, 220)
                );
                using (Brush brush = new SolidBrush(color))
                {
                    graphics.FillEllipse(brush, x, y, size, size);
                }
            }

            // Случайные мелкие линии на фоне
            for (int i = 0; i < 10; i++)
            {
                Color color = Color.FromArgb(
                    random.Next(180, 230),
                    random.Next(180, 230),
                    random.Next(180, 230)
                );
                using (Pen pen = new Pen(color, 1))
                {
                    int x1 = random.Next(width);
                    int y1 = random.Next(height);
                    int x2 = random.Next(width);
                    int y2 = random.Next(height);
                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        /// <summary>
        /// Рисует текст CAPTCHA с искажениями
        /// </summary>
        private void DrawCaptchaText(Graphics graphics, int width, int height)
        {
            string[] fontFamilies = { "Arial", "Verdana", "Times New Roman", "Courier New", "Georgia" };

            int charWidth = width / (CaptchaText.Length + 1);
            int startX = charWidth / 2;

            for (int i = 0; i < CaptchaText.Length; i++)
            {
                // Случайный шрифт для каждого символа
                string fontFamily = fontFamilies[random.Next(fontFamilies.Length)];
                int fontSize = random.Next(20, 30);
                FontStyle fontStyle = random.Next(2) == 0 ? FontStyle.Bold : FontStyle.Italic;

                using (Font font = new Font(fontFamily, fontSize, fontStyle))
                {
                    // Случайный цвет (тёмные оттенки для читаемости)
                    Color color = Color.FromArgb(
                        random.Next(0, 100),
                        random.Next(0, 100),
                        random.Next(0, 100)
                    );

                    using (Brush brush = new SolidBrush(color))
                    {
                        // Позиция с вертикальным смещением (символы НЕ на одной линии)
                        int x = startX + i * charWidth;
                        int y = random.Next(5, height - 35);

                        // Применение поворота для каждого символа
                        float angle = random.Next(-25, 25);

                        // Сохраняем состояние
                        var state = graphics.Save();

                        // Перемещаем центр вращения
                        graphics.TranslateTransform(x + 10, y + 15);
                        graphics.RotateTransform(angle);

                        // Рисуем символ
                        graphics.DrawString(
                            CaptchaText[i].ToString(),
                            font,
                            brush,
                            -10, -15
                        );

                        // Восстанавливаем состояние
                        graphics.Restore(state);
                    }
                }
            }
        }

        /// <summary>
        /// Рисует линии-помехи (перечёркивание)
        /// </summary>
        private void DrawNoiseLines(Graphics graphics, int width, int height)
        {
            // Основные перечёркивающие линии
            for (int i = 0; i < 5; i++)
            {
                Color color = Color.FromArgb(
                    random.Next(50, 150),
                    random.Next(50, 150),
                    random.Next(50, 150)
                );

                int penWidth = random.Next(1, 3);
                using (Pen pen = new Pen(color, penWidth))
                {
                    // Волнистые линии через Bezier
                    Point start = new Point(random.Next(0, 20), random.Next(height));
                    Point end = new Point(width - random.Next(0, 20), random.Next(height));
                    Point control1 = new Point(width / 3, random.Next(height));
                    Point control2 = new Point(2 * width / 3, random.Next(height));

                    graphics.DrawBezier(pen, start, control1, control2, end);
                }
            }

            // Дополнительные короткие линии
            for (int i = 0; i < 3; i++)
            {
                Color color = Color.FromArgb(
                    random.Next(80, 180),
                    random.Next(80, 180),
                    random.Next(80, 180)
                );

                using (Pen pen = new Pen(color, 2))
                {
                    int x1 = random.Next(width);
                    int y1 = random.Next(height);
                    int x2 = x1 + random.Next(-50, 50);
                    int y2 = y1 + random.Next(-20, 20);
                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        /// <summary>
        /// Рисует передний шум поверх текста
        /// </summary>
        private void DrawForegroundNoise(Graphics graphics, int width, int height)
        {
            for (int i = 0; i < 50; i++)
            {
                int x = random.Next(width);
                int y = random.Next(height);
                int size = random.Next(1, 3);
                Color color = Color.FromArgb(
                    random.Next(100, 200),
                    random.Next(100, 200),
                    random.Next(100, 200)
                );
                using (Brush brush = new SolidBrush(color))
                {
                    graphics.FillEllipse(brush, x, y, size, size);
                }
            }
        }

        /// <summary>
        /// Проверяет введённый текст
        /// </summary>
        /// <param name="input">Введённый пользователем текст</param>
        /// <returns>True если совпадает (без учёта регистра)</returns>
        public bool ValidateCaptcha(string input)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(CaptchaText))
                return false;

            return string.Equals(input.Trim(), CaptchaText, StringComparison.OrdinalIgnoreCase);
        }
    }
}