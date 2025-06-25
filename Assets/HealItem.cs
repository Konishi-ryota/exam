using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] int HouseHealPower;
    [SerializeField] int HeroHealPower;
    [SerializeField] int _beforeRemove = 10;

    private House _house;
    private Hero _hero;
    // Start is called before the first frame update
    void Start()
    {
        _house = FindAnyObjectByType<House>();
        _hero = FindAnyObjectByType<Hero>();
        Destroy(gameObject, _beforeRemove);//n�b��ɏ���
    }
    private void OnTriggerEnter2D(Collider2D collision)//isTrigger���Ă�̂�TriggerEnter
    {
        if (collision.gameObject.tag == "Player")//�v���C���[�Ɠ���������
            //�^�O�g���Ă�R�[�h���y�Ɍ�����Ƒ�́u�^�O�g���Ă�̂�...�v���Č�����
            //���ɂǂ�Ȃ���������̂��͒��ד������A���̂����^�C�s���O�~�X���Ď���
        {
            _house.HouseHP += HouseHealPower;
            _hero.H_HP += HeroHealPower;//�v���C���[�ƉƂ�HP�𑝂₷
            Destroy(this.gameObject);
            _house.SetHouseHP();
            _hero.SetHP();//���ꂼ��̗̑͊Ǘ����\�b�h�Ăяo��
            Debug.Log($"{_house.HouseHP}");
        }
    }
}
