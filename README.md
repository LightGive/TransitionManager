# TransitionManager [![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?style=flat)](http://mit-license.org)<br>
It is a manager of scene transitions using Unity's uGUI.<br>
You can make 16 kinds of + Image fade transition effect by changing ImageType of Image.<br>
We also made it possible to use the production with the rule image set.<br>
It is confirmed in iOS, Android, WebGL.<br>
Inspector extension is also done and it is easy to use<br>
You can also see what kind of action you will take from the link below.<br><br>
[TransitionManagerExample](https://lightgive.github.io/MyPage/Examples/TransitionManagerExample/index.html)<br>

## Update <br>
<dl>
  <dt>ver1.2.0(2018.11.07)</dt>
    <dd>・Added Flash function</dd>
    <dd>・Change to LoadScene so that it can be set with argument instead of default value</dd>
  <dt>ver1.1.1(2018.09.18)</dt>
  <dd>・Inspector extension such as displaying preview of rule image</dd>
  <dt>ver1.1.0(2018.09.17)</dt>
  <dd>・Added so that scene transition can be made with rule image</dd>
</dl>

## Example <br>

### UI_Transition<br>
<img src="https://78.media.tumblr.com/1a6ae7adbbb33c3a4cfeab020fb5b161/tumblr_pf8f678GVz1u4382eo1_400.gif" alt="シーン遷移サンプル1" title="サンプル"><img src="https://78.media.tumblr.com/72e838e53edbf4d56494e5af5ea74d9f/tumblr_pf8f678GVz1u4382eo2_400.gif" alt="シーン遷移サンプル2" title="サンプル"><br>
<img src="https://78.media.tumblr.com/cb19b6219ad75b62a7e576b43d2eb040/tumblr_pf8f678GVz1u4382eo3_400.gif" alt="シーン遷移サンプル3" title="サンプル"><img src="https://78.media.tumblr.com/9659dd627bf835c7bda6fb3275004add/tumblr_pf8f678GVz1u4382eo4_400.gif" alt="シーン遷移サンプル4" title="サンプル"><br>
<img src="https://78.media.tumblr.com/88f456d4dd9f56744a124fb7a59f386d/tumblr_pf8f678GVz1u4382eo5_400.gif" alt="シーン遷移サンプル5" title="サンプル">

### Fade Transition<br>
<img src="https://78.media.tumblr.com/1fdbab5844b8df4bdb77b7e4eaf49954/tumblr_pf8f678GVz1u4382eo7_400.gif" alt="シーン遷移サンプル6" title="サンプル">

### Rule Texture Transition<br>
<img src="https://78.media.tumblr.com/dbee1b043471c699243e7b7d5ebe182b/tumblr_pf8f678GVz1u4382eo6_400.gif" alt="シーン遷移サンプル7" title="サンプル">

## InspectorSetting
<img src="https://66.media.tumblr.com/2cbd3c929bd64ffadc7bbea41c6fb0f3/tumblr_phshytOlSw1u4382eo1_400.gif">

## SampleScript

```csharp
public class Test
{
    void Start()
    {
        TransitionManager.Instance.LoadScene("SceneName");
    }
}
```

## UnityVersion
Unity 2018.2.8f1<br>

## License
See [LICENSE](/LICENSE).



