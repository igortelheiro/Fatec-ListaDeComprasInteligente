#!bin/bash

# DEPENDENCIES
apt-get update
apt install -y gconf-service libasound2 libatk1.0-0 libc6 libcairo2 libcups2 libdbus-1-3 libexpat1 libfontconfig1 libgcc1 libgconf-2-4 libgdk-pixbuf2.0-0 libglib2.0-0 libgtk-3-0 libnspr4 libpango-1.0-0 libpangocairo-1.0-0 libstdc++6 libx11-6 libx11-xcb1 libxcb1 libxcomposite1 libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 libxrandr2 libxrender1 libxss1 libxtst6 ca-certificates fonts-liberation libappindicator1 libnss3 lsb-release xdg-utils wget
#apt install -y libgobject-2.0.so.0 libglib-2.0.so.0 libnss3.so libnssutil3.so libsmime3.so libnspr4.so libatk-1.0.so.0 libatk-bridge-2.0.so.0 libcups.so.2 libdrm.so.2 libdbus-1.so.3 libgio-2.0.so.0 libexpat.so.1 libxcb.so.1 libxkbcommon.so.0 libX11.so.6 libXcomposite.so.1 libXdamage.so.1 libXext.so.6 libXfixes.so.3 libXrandr.so.2 libgbm.so.1 libgtk-3.so.0 libgdk-3.so.0 libpango-1.0.so.0 libcairo.so.2 libasound.so.2 libatspi.so.0 libxshmfence.so.1

# CHROME
#wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add -
#echo "deb http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list
#apt-get update && apt-get -y install google-chrome-stable