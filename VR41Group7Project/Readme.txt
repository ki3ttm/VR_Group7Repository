[シーン遷移使い方]
Prefabsに入っている[SceneManager]と[EffectCanvas]をヒエラルキーに入れる
[SceneManager]のFadeに[EffectCanvas]の中Fadeという名前のゲームオブジェクトを入れる

[説明]
ヒエラルキーの[SceneManager]でシーンを管理しています
[SceneManager]の中に読み込みたい順番通りに上から入れていきます
例)タイトル → ステージセレクト → ゲーム → リザルトの順番に読み込む！

▼[SceneManager]
　▼Title
　▼StageSelect
　▼GameMain
　▼Result

の順番に入れれば大丈夫です。

削除や追加や読み込む順番を変える場合SceneManagerのスクリプトの列挙子を
読み込む順番に変えてください。

TitleやStageSelectなどは一つ一つにスクリプトを作り分けてその中で処理をしている
感じにしています。
シーンを切り替えるには
Titleの場合Titleスクリプトから[SceneManager]の中にあるスクリプトを参照して、
SceneChange(SceneManagerにある列挙子)を呼び出すことによりシーンを切り替えることができます。

シーンを切り替えるたびにScene関連のオブジェクトの中に入ったものをすべて消します。
なので背景などシーンをまだいても消したくないものがあればScene関連の中に
オブジェクトは置かないでください。
シーンをまたがない場合はScene関連の中でオブジェクトを生成するようにしてください。

▼[SceneManager]
　▼Title(シーンを切り替えてもここは消えない)
　　▼TitleText(シーンを切り替えるとここが消える)
　▼StageSelect
　▼GameMain
　▼Result

初期化はOnEnable()関数の中で行うと良いです