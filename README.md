# TransitionManager [![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?style=flat)](http://mit-license.org)<br>
UnityのUIを使用したシーン遷移のマネージャー<br>
ImageのImageTypeを変えた16種類＋フェードの遷移演出をする事が出来ます。<br>
ルール画像を設定したトランジションも可能。<br>

## Update <br>
ver 1.1.0:ルール画像でシーン遷移出来るように追加(2018.09.17)<br>
ver 1.1.1:ルール画像のプレビューを表示するなどインスペクター拡張(2018.09.18)<br>

## Example <br>
シーンの遷移の表示例<br>

### UI_Transition<br>
<img src="https://78.media.tumblr.com/1a6ae7adbbb33c3a4cfeab020fb5b161/tumblr_pf8f678GVz1u4382eo1_400.gif" alt="シーン遷移サンプル1" title="サンプル"><img src="https://78.media.tumblr.com/72e838e53edbf4d56494e5af5ea74d9f/tumblr_pf8f678GVz1u4382eo2_400.gif" alt="シーン遷移サンプル2" title="サンプル"><br>
<img src="https://78.media.tumblr.com/cb19b6219ad75b62a7e576b43d2eb040/tumblr_pf8f678GVz1u4382eo3_400.gif" alt="シーン遷移サンプル3" title="サンプル"><img src="https://78.media.tumblr.com/9659dd627bf835c7bda6fb3275004add/tumblr_pf8f678GVz1u4382eo4_400.gif" alt="シーン遷移サンプル4" title="サンプル"><br>
<img src="https://78.media.tumblr.com/88f456d4dd9f56744a124fb7a59f386d/tumblr_pf8f678GVz1u4382eo5_400.gif" alt="シーン遷移サンプル5" title="サンプル">

### Fade Transition<br>
<img src="https://78.media.tumblr.com/1fdbab5844b8df4bdb77b7e4eaf49954/tumblr_pf8f678GVz1u4382eo7_400.gif" alt="シーン遷移サンプル6" title="サンプル">

### Rule Texture Transition<br>
<img src="https://78.media.tumblr.com/dbee1b043471c699243e7b7d5ebe182b/tumblr_pf8f678GVz1u4382eo6_400.gif" alt="シーン遷移サンプル7" title="サンプル">

## InspectorSetting
| プロパティ | 説明 |
|:---|:---|
| TransitionType | 遷移の種類 |
| Duration | 遷移時間 |
| TexColor | 背景の色 |
| AnimCurve| 遷移時間のカーブ |

<img src="https://78.media.tumblr.com/0075610ec93360c88caa2c9cbf627d2d/tumblr_pf8g7lwe8b1u4382eo1_400.gif" alt="シーン遷移サンプル7" title="サンプル">

## SampleScript

```csharp
public class Test
{
    void Start()
    {
        TransitionManager.Instance.LoadLevel("SceneName");
    }
}
```

## UnityVersion
Unity 2018.1.5f1<br>

## License
See [LICENSE](/LICENSE).

