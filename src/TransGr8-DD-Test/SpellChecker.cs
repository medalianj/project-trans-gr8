namespace TransGr8_DD_Test
{
    public class SpellChecker
	{
		private readonly List<Spell> _spellList;

		public SpellChecker(List<Spell> spells)
		{
			_spellList = spells;
		}

		public bool CanUserCastSpell(User user, string spellName)
		{
            if (_spellList == null)
            {
                throw new ArgumentNullException(nameof(_spellList));
            }

            Spell spell = _spellList.Find(s => s.Name == spellName || s.Name.Contains(spellName));

            var isUserAllowedToCastSpell = (user != null && spell != null) ? VerifyUserAbilityToCastSpell(user, spell) : false;

            return isUserAllowedToCastSpell;
        }

		public bool VerifyUserAbilityToCastSpell(User user, Spell spell)
		{
            var isUserLevelOrRangeLowerThanSpell = new UserLevelAndRangeChecker().Verify(user, spell);
            if (isUserLevelOrRangeLowerThanSpell)
            {
                return false;
            }

            var isSpellComponentAndUserAbilitiesUnmatching = new SpellComponentsAndUserAbilitiesChecker().Verify(user, spell);
            if (isSpellComponentAndUserAbilitiesUnmatching)
            {
                return false;
            }

            var isSpellDurationAndUserAbilitiesUnmatching = new SpellComponentsAndUserAbilitiesChecker().Verify(user, spell);
            if (isSpellDurationAndUserAbilitiesUnmatching)
            {
                return false;
            }
            return true;
        }
    }

    public interface IChecker
    {
        bool Verify(User user, Spell spell);
    }

    public class UserLevelAndRangeChecker : IChecker
    {
        public bool Verify(User user, Spell spell)
        {
            return user.Level < spell.Level || user.Range < spell.Range;
        }
    }

    public class SpellComponentsAndUserAbilitiesChecker : IChecker
    {
        public bool Verify(User user, Spell spell)
        {
            if (spell.Components == null)
            {
                throw new ArgumentNullException(nameof(spell.Components));
            }

            if (spell.Components.Contains("V") && !user.HasVerbalComponent)
            {
                return true;
            }
            else if (spell.Components.Contains("S") && !user.HasSomaticComponent)
            {
                return true;
            }
            else if (spell.Components.Contains("M") && !user.HasMaterialComponent)
            {
                return true;
            }

            return false;
        }
    }

    public class SpellDurationAndUserAbilitiesChecker : IChecker
    {
        public bool Verify(User user, Spell spell)
        {
            return spell.Duration.Contains("Concentration") && !user.HasConcentration;
        }
    }
}
