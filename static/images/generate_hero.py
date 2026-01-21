from PIL import Image, ImageDraw, ImageFont
import os

# Create a new image with the specified dimensions
width = 1000
height = 420
bg_color = (15, 23, 42)  # Dark blue-gray background

# Create the base image
img = Image.new('RGB', (width, height), bg_color)
draw = ImageDraw.Draw(img)

# Load the temperature sensor chart
sensor_img_path = r"d:\source\repos\robjohnston\static\images\solid\temperature_sensor_fast-001.png"
sensor_img = Image.open(sensor_img_path)

# Resize the sensor image to fit nicely (keeping aspect ratio)
sensor_width = 500
sensor_height = int(sensor_img.height * (sensor_width / sensor_img.width))
sensor_img = sensor_img.resize((sensor_width, sensor_height), Image.Resampling.LANCZOS)

# Position the sensor image on the right side
sensor_x = width - sensor_width - 20
sensor_y = (height - sensor_height) // 2
img.paste(sensor_img, (sensor_x, sensor_y), sensor_img if sensor_img.mode == 'RGBA' else None)

# Add gradient overlay on the left for text readability
for x in range(sensor_x):
    alpha = int(255 * (1 - (x / sensor_x) * 0.3))
    for y in range(height):
        current = img.getpixel((x, y))
        img.putpixel((x, y), (
            min(255, current[0] + (bg_color[0] - current[0]) * alpha // 255),
            min(255, current[1] + (bg_color[1] - current[1]) * alpha // 255),
            min(255, current[2] + (bg_color[2] - current[2]) * alpha // 255)
        ))

# Try to use a nice font, fallback to default if not available
try:
    title_font = ImageFont.truetype("arial.ttf", 48)
    subtitle_font = ImageFont.truetype("arial.ttf", 28)
    small_font = ImageFont.truetype("arial.ttf", 20)
except:
    title_font = ImageFont.load_default()
    subtitle_font = ImageFont.load_default()
    small_font = ImageFont.load_default()

# Add text on the left side
text_color = (226, 232, 240)  # Light gray
accent_color = (147, 197, 253)  # Light blue
warning_color = (248, 113, 113)  # Light red

# Main title
title_text = "Liskov Substitution"
title_y = 80
draw.text((40, title_y), title_text, fill=text_color, font=title_font)

title_text2 = "Principle"
draw.text((40, title_y + 55), title_text2, fill=text_color, font=title_font)

# Subtitle
subtitle_y = title_y + 130
draw.text((40, subtitle_y), "When Your Cheap Sensor", fill=accent_color, font=subtitle_font)
draw.text((40, subtitle_y + 35), "Breaks Everything", fill=warning_color, font=subtitle_font)

# SOLID badge
badge_y = height - 60
draw.text((40, badge_y), "SOLID PRINCIPLES", fill=(100, 116, 139), font=small_font)

# Draw decorative line
line_y = subtitle_y - 20
draw.rectangle([(40, line_y), (280, line_y + 3)], fill=accent_color)

# Save the image
output_path = r"d:\source\repos\robjohnston\static\images\solid\liskov_hero.png"
img.save(output_path, 'PNG', optimize=True)
print(f"Hero image created: {output_path}")
print(f"Dimensions: {width}x{height}")
