import Vue from 'vue'
import './plugins/axios'
import i18n from './plugins/i18n'
import vuetify from './plugins/vuetify'
import App from './App.vue'
import Chart from 'chart.js'
import Chartkick from 'vue-chartkick'
import router from './router'
import store from '@/store/index'
import './registerServiceWorker'
import dateFilter from '@/filters/date.filter'
import moment from 'moment'

Vue.use(Chartkick.use(Chart))

Vue.config.productionTip = false

Vue.filter('date', dateFilter)
Vue.filter('formatDate', function (value: string) {
  if (value) {
    return moment(value).fromNow()
  }
})
Vue.filter('numFormatter', function (num: number) {
  if (num > 1000000000) {
    return `${(num / 1000000000).toFixed(1)}B`
  } else if (num > 1000000) {
    return `${(num / 1000000).toFixed(1)}M`
  } else if (num > 999 && num < 1000000) {
    return `${(num / 1000).toFixed(1)}K`
  } else if (num < 900) {
    return `${num}`
  }
})

new Vue({
  i18n,
  vuetify,
  router,
  store,
  render: (h) => h(App)
}).$mount('#app')
