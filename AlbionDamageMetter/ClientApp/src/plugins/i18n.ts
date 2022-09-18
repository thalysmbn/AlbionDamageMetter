import Vue from 'vue'
import VueI18n from 'vue-i18n'
import en from '../locales/en'

Vue.use(VueI18n)
const i18n = new VueI18n({
  locale: localStorage.lang || 'en',
  messages: {
    en: en
  }
})

export default i18n
