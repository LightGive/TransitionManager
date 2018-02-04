# TransitionManager [![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?style=flat)](http://mit-license.org)<br>
UnityのUIを使用したシーン遷移のマネージャー。<br>
ImageのImageTypeを変えて色んな遷移方法を実装しています。<br>


## SceneTransition Sample<br>
<img src="https://78.media.tumblr.com/14349e6ca31f887e7d8a84703312cdbe/tumblr_p3lwx73LWQ1u4382eo1_1280.gif" alt="シーン遷移サンプル1" title="サンプル"><br>
<img src="https://78.media.tumblr.com/ef5c3f6ff4c17df92d8bf2ad35cd0467/tumblr_p3lwxkHmiE1u4382eo1_1280.gif" alt="シーン遷移サンプル2" title="サンプル"><br>
<img src="https://78.media.tumblr.com/1eacd7ba93eaa2ce63350978cff9d7bc/tumblr_p3lwxvaRZW1u4382eo1_1280.gif" alt="シーン遷移サンプル3" title="サンプル"><br>
<img src="https://78.media.tumblr.com/047f8092db3bd2797ee806804b4fb43c/tumblr_p3lwynxVhg1u4382eo1_1280.gif" alt="シーン遷移サンプル4" title="サンプル"><br>
<img src="https://78.media.tumblr.com/501edb93fc8c1f3c29371f3d5d46c1e6/tumblr_p3lwyxzqkN1u4382eo1_1280.gif" alt="シーン遷移サンプル5" title="サンプル"><br>
<img src="https://78.media.tumblr.com/d2894885bdc4ffb8fdf52d5ca09ec381/tumblr_p3lwz7zTk91u4382eo1_1280.gif" alt="シーン遷移サンプル6" title="サンプル"><br>

## 使い方
1. Create gameObject.<br>
2. Add component "TransitionManager".<br>
<img src="https://78.media.tumblr.com/d2894885bdc4ffb8fdf52d5ca09ec381/tumblr_p3lwz7zTk91u4382eo1_1280.gif" alt="シーン遷移サンプル6" title="サンプル"><br>

### SettingInspector 
| Property | Description |
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

