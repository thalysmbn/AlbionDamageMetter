import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import Home from '../views/Home.vue'
import Cluster from '../views/Cluster.vue'
import Clusters from '../views/Clusters.vue'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/clusters',
    name: 'Clusters',
    component: Clusters
  },
  {
    path: '/cluster/:date',
    name: 'Cluster',
    component: Cluster
  }
]

const router = new VueRouter({
  mode: 'history',
  routes
})

export default router
