[シーン遷移使い方]
Prefabsに入っている[StageManager]と[EffectCanvas]をヒエラルキーに入れる
[StageManager]のFadeに[EffectCanvas]の中Fadeという名前のゲームオブジェクトを入れる

[説明]
ヒエラルキーの[StageManager]でシーンを管理しています
[stageManager]の中に読み込みたい順番通りに上から入れていきます
例)タイトル → ステージセレクト → ゲーム → リザルトの順番に読み込む！

▼[StageManager]
　▼Title
　▼StageSelect
　▼GameMain
　▼Result

の順番に入れれば大丈夫です。

削除や追加や読み込む順番を変える場合StageManagerのスクリプトの列挙子を
読み込む順番に変えてください。

TitleやStageSelectなどは一つ一つにスクリプトを作り分けてその中で処理をしている
感じにしています。
シーンを切り替えるには
Titleの場合Titleスクリプトから[StageManager]の中にあるスクリプトを参照して、
SceneChange(StageManagerにある列挙子)を呼び出すことによりシーンを切り替えることができます。