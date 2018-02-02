var path = require('path');

module.exports = {
    entry: {
        main: './Scripts/index'
    },
    output: {
       path: path.join(__dirname, '/wwwroot/js/'),
       filename: 'main.build.js'
    },
   module: {
         rules: [
           {
             test: /\.css$/,
             use: [
               'style-loader',
               'css-loader'
             ]
           }
         ]
        }
       };