# calcHLAC-frontend
画像の領域を指定してその領域のHLAC特徴量を得るためのGUIソフトウェア

.NET Framework 4.5(C#) + OpenCVSharp で開発

バグあるかも

基本的に私得なツールなので、特に詳しくは書かないですが…

## 基本機能
- フォルダを指定して画像ファイル群を読み込み
- 演算対象領域を指定可能
- 二値化パラメーター等も指定可能
- ステップサイズを複数指定可能
- 設定や読み込んだ画像等の情報をファイルに書き出し/読み込み可能

## 領域指定
### ドラックで描く
![領域描画](http://i.gyazo.com/2f3239f9f79f6e2f79a918f726e239e8.gif "領域描画")

### 移動可能
![領域移動](http://i.gyazo.com/dce899d49b95a9fafbc33759dea0db73.gif "領域移動")

### ダブルクリックで削除
![領域削除](http://i.gyazo.com/047a243bcaa53a25ba9ea3018b183edc.gif "領域削除")


## 他の操作
### Ctrl+マウスホイール で拡大縮小
![拡大縮小](http://i.gyazo.com/5b616af672911c1ca09acf031fb46b72.gif "拡大縮小")

### 右クリックドラッグで画像表示位置を移動
![移動](http://i.gyazo.com/c43c075bcaa4cb12e038a6b94c545d6c.gif "移動")

### Shift+J,K で表示画像切り替え
![画像切り替え](http://i.gyazo.com/04c9613c01246bdc98f71e648a2e4326.gif "画像切り替え")


## ライセンス
MIT
