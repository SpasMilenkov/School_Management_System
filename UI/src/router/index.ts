import { createRouter, createWebHistory } from 'vue-router'
import { useUserStore } from '../stores/UserStore.ts'
import Home from '../views/HomeView.vue'
import Login from '../views/LoginView.vue'
import Dashboard from '../views/DashboardView.vue'
import OverviewView from '../views/StudentViews/StudentOverviewView.vue'
import GradesView from '../views/StudentViews/GradesView.vue'
import AbsenceView from '../views/StudentViews/AbsenceView.vue'
import TeachersView from '../views/TeacherViews/TeachersView.vue'
import SubjectsView from '../views/AdminViews/SubjectsView.vue'
import AdminView from '../views/AdminViews/AdminView.vue'
// import ErrorComponent from '../components/general/ErrorComponent.vue'
import WaitingPage from '../views/AdminViews/WaitingApprovalView.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: Home,
      meta: {
        requiresAuthentication: false
      }
    },
    {
      path: '/waiting',
      component: WaitingPage,
      meta: {
        requiresAuthentication: false
      }
    },
    {
      path: '/login',
      component: Login,
      meta: {
        requiresAuthentication: false
      }
    },
    {
      path: '/dashboard',
      component: Dashboard,
      meta: {
        requiresAuthentication: true
      },
      children: [
        {
          path: 'student', // Remove the leading slash (/)
          meta: {
            requiresAuthentication: true
          },
          children: [
            {
              path: 'overview', // Remove the leading slash (/)
              component: OverviewView,
              meta: {
                requiresAuthentication: true
              }
            },
            {
              path: 'grades', // Remove the leading slash (/)
              component: GradesView,
              meta: {
                requiresAuthentication: true
              }
            }
          ]
        },
        {
          path: 'teacher', // Remove the leading slash (/)
          meta: { requiresAuthentication: true },
          children: [
            {
              path: 'overview', // Remove the leading slash (/)
              component: TeachersView,
              meta: { requiresAuthentication: true }
            }
          ]
        },
        {
          path: 'admin', // Remove the leading slash (/)
          meta: { requiresAuthentication: true },
          children: [
            {
              path: 'overview', // Remove the leading slash (/)
              component: AdminView,
              meta: { requiresAuthentication: true }
            },
            {
              path: 'subjects',
              component: SubjectsView,
              meta: { requiresAuthentication: true }
            }
          ]
        },
        {
          path: 'absence', // Remove the leading slash (/)
          component: AbsenceView,
          meta: {
            requiresAuthentication: true
          }
        },
        {
          path: 'teachers', // Remove the leading slash (/)
          component: TeachersView,
          meta: {
            requiresAuthentication: true
          }
        }
      ]
    }
  ]
})

router.beforeEach((to, from, next) => {
  if (to.matched.some((page) => page.meta.requiresAuthentication)) {
    const store = useUserStore()

    if (!store.authenticationStatus)
      next({
        path: '/login',
        component: Login
      })
    else next()
  } else next()
})
export default router