# TransitionManager [![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?style=flat)](http://mit-license.org)<br>
UnityのUIを使用したシーン遷移のマネージャー<br>
ImageのImageTypeを変えた16種類＋フェードの遷移演出をする事が出来ます。<br>
ルール画像を設定したトランジションも可能。<br>

## Example <br>

### UI_Transition<br>
<img src="https://78.media.tumblr.com/1a6ae7adbbb33c3a4cfeab020fb5b161/tumblr_pf8f678GVz1u4382eo1_400.gif" alt="シーン遷移サンプル1" title="サンプル"><img src="https://78.media.tumblr.com/72e838e53edbf4d56494e5af5ea74d9f/tumblr_pf8f678GVz1u4382eo2_400.gif" alt="シーン遷移サンプル2" title="サンプル"><br>
<img src="https://78.media.tumblr.com/cb19b6219ad75b62a7e576b43d2eb040/tumblr_pf8f678GVz1u4382eo3_400.gif" alt="シーン遷移サンプル3" title="サンプル"><img src="https://78.media.tumblr.com/9659dd627bf835c7bda6fb3275004add/tumblr_pf8f678GVz1u4382eo4_400.gif" alt="シーン遷移サンプル4" title="サンプル"><br>
<img src="https://78.media.tumblr.com/88f456d4dd9f56744a124fb7a59f386d/tumblr_pf8f678GVz1u4382eo5_400.gif" alt="シーン遷移サンプル5" title="サンプル">

### Fade Transition<br>
<img src="https://78.media.tumblr.com/1fdbab5844b8df4bdb77b7e4eaf49954/tumblr_pf8f678GVz1u4382eo7_400.gif" alt="シーン遷移サンプル6" title="サンプル">

### Rule Texture Transition<br>
<img src="https://78.media.tumblr.com/dbee1b043471c699243e7b7d5ebe182b/tumblr_pf8f678GVz1u4382eo6_400.gif" alt="シーン遷移サンプル7" title="サンプル">

## 使い方
1. GameObjectを作成.<br>
2. "TransitionManager.cs"をAdd Componentする<br>
<br>
<img src="https://78.media.tumblr.com/8393804e5f253b6e5f00d6b9a13a6589/tumblr_p3lyieVtxx1u4382eo1_1280.png" alt="シーン遷移サンプル6" title="サンプル"><br>

### インスペクターの設定 
| プロパティ | 説明 |
|:---|:---|
| TransitionTime | シーンの遷移時間 |
| TransitionColor | シーン遷移時の背景の色 |
| TransitionType | シーン遷移の方法 |
| AnimationCurve| シーン遷移のアニメーションカーブ |

### スクリプトの実装
```csharp
public class Test
{
    void Start()
    {
        TransitionManager.Instance.LoadLevel("SceneName");
    }
}
```

## 開発環境
Unity 2017.3.0f3<br>

## License
See [LICENSE](/LICENSE).

