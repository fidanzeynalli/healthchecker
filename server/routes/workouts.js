const express = require('express');
const router = express.Router();

let workouts = [];
let nextId = 1;

// GET all workouts
router.get('/', (req, res) => {
  res.json(workouts);
});

// GET a single workout by id
router.get('/:id', (req, res) => {
  const workout = workouts.find(w => w.id === parseInt(req.params.id));
  if (!workout) return res.status(404).json({ error: 'Workout not found' });
  res.json(workout);
});

// CREATE a new workout
router.post('/', (req, res) => {
  const { name, duration, date } = req.body;
  if (!name || !duration || !date) {
    return res.status(400).json({ error: 'Missing required fields' });
  }
  const workout = { id: nextId++, name, duration, date };
  workouts.push(workout);
  res.status(201).json(workout);
});

// UPDATE a workout
router.put('/:id', (req, res) => {
  const { name, duration, date } = req.body;
  const workout = workouts.find(w => w.id === parseInt(req.params.id));
  if (!workout) return res.status(404).json({ error: 'Workout not found' });
  if (name !== undefined) workout.name = name;
  if (duration !== undefined) workout.duration = duration;
  if (date !== undefined) workout.date = date;
  res.json(workout);
});

// DELETE a workout
router.delete('/:id', (req, res) => {
  const id = parseInt(req.params.id);
  const index = workouts.findIndex(w => w.id === id);
  if (index === -1) return res.status(404).json({ error: 'Workout not found' });
  const deleted = workouts.splice(index, 1);
  res.json(deleted[0]);
});

module.exports = router; 