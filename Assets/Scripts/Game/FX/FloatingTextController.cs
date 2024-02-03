using UnityEngine;
using QFramework;
using UnityEngine.UI;

namespace StarScavenger
{
    public enum TextType
    {
        Default,
        HP,
        Fuel,
        Coin,
        Discover,
        Arrive,
        Attack,
    }

    public partial class FloatingTextController : ViewController
    {
        private static FloatingTextController mDefault;

        private void Awake()
        {
            mDefault = this;
        }

        private void Start()
        {
            FloatingText.Hide();
        }

        public static void Play(string text, TextType type = TextType.Default)
        {
            Player player = Player.Default;
            Vector3 playerPos = player.transform.position + player.transform.up * (Global.CurrentSpeed.Value * 0.5f + 1f);

            mDefault.FloatingText.InstantiateWithParent(mDefault.transform)
                .PositionX(playerPos.x)
                .PositionY(playerPos.y)
                .LocalEulerAngles(player.transform.localEulerAngles)
                .Self(f =>
                {
                    Transform textTrans = f.transform.Find("Text");
                    Text textComp = textTrans.GetComponent<Text>();
                    textComp.text = text;

                    switch (type)
                    {
                        case TextType.Default:
                            textComp.color = Color.white;
                            break;
                        case TextType.HP:
                            textComp.color = Color.red;
                            break;
                        case TextType.Fuel:
                            textComp.color = Color.blue;
                            break;
                        case TextType.Coin:
                            textComp.color = new Color32(255, 215, 0, 255); // 金色
                            break;
                        case TextType.Discover:
                            textComp.color = Color.green;
                            break;
                        case TextType.Arrive:
                            textComp.color = Color.cyan;
                            break;
                        case TextType.Attack:
                            textComp.color = Color.yellow;
                            break;
                        default:
                            textComp.color = Color.white;
                            break;
                    }

                    float positionY = playerPos.y;

                    // 动作序列
                    ActionKit.Sequence()
                    .Lerp(0, 0.5f, 0.5f, p =>   // 向上飘动并变大
                    {
                        f.PositionY(positionY + p * 0.3f);
                        textComp.LocalScaleX(Mathf.Clamp01(p * 5f));
                        textComp.LocalScaleY(Mathf.Clamp01(p * 5f));
                    })
                    .Delay(0.5f) // 等待 0.5 秒
                    .Lerp(1f, 0, 0.3f, p => // 变透明
                    {
                        textComp.ColorAlpha(p);

                    }, () => // Lerp 完成之后的回调
                    {
                        textTrans.parent.DestroyGameObjGracefully();
                        //TODO 待优化
                    })
                    .Start(textComp); // 将其生命周期 Start 绑定给 textComp

                }).Show();
        }

        private void OnDestroy()
        {
            mDefault = null;
        }
    }
}
