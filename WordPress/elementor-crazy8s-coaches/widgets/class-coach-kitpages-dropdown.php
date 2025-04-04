<?php

/**
 * Coach KitPages Dropdown class.
 *
 * @category   Class
 * @package    ElementorCrazy8sCoaches
 * @subpackage WordPress
 * @author     Dug Steen <dug@softcrow.net>
 * @copyright  2025 Dug Steen
 * @license    https://opensource.org/licenses/GPL-3.0 GPL-3.0-only
 * @since      1.0.0
 * php version 8.1
 */

namespace ElementorCrazy8sCoaches\Widgets;

use Elementor\Widget_Base;
use Elementor\Controls_Manager;

// Security Note: Blocks direct access to the plugin PHP files.
defined('ABSPATH') || die();

/**
 * CoachKitPagesDropdown widget class.
 *
 * @since 1.0.0
 */
class CoachKitPagesDropdown extends Widget_Base
{
	/**
	 * Class constructor.
	 *
	 * @param array $data Widget data.
	 * @param array $args Widget arguments.
	 */
	public function __construct($data = array(), $args = null)
	{
		parent::__construct($data, $args);

		wp_register_style('crazy8s-elementor', plugins_url('/assets/css/crazy8s-elementor.css', ELEMENTOR_CRAZY8S_COACHES), array(), '1.0.2');
	}

	/**
	 * Retrieve the widget name.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget name.
	 */
	public function get_name()
	{
		return 'coach-kitpages-dropdown';
	}

	/**
	 * Retrieve the widget title.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget title.
	 */
	public function get_title()
	{
		return __('Coach Kit Pages Dropdown', 'elementor-crazy8s-coaches');
	}

	/**
	 * Retrieve the widget icon.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget icon.
	 */
	public function get_icon()
	{
		return 'fa fa-list-dropdown';
	}

	/**
	 * Retrieve the list of categories the widget belongs to.
	 *
	 * Used to determine where to display the widget in the editor.
	 *
	 * Note that currently Elementor supports only one category.
	 * When multiple categories passed, Elementor uses the first one.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return array Widget categories.
	 */
	public function get_categories()
	{
		return array('crazy8s');
	}

	/**
	 * Enqueue styles.
	 */
	public function get_style_depends()
	{
		return array('crazy8s-elementor');
	}

	/**
	 * Register the widget controls.
	 *
	 * Adds different input fields to allow the user to change and customize the widget settings.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function _register_controls()
	{
		$this->start_controls_section(
			'section_content',
			array(
				'label' => __('Content', 'elementor-crazy8s-coaches'),
			)
		);

		$this->end_controls_section();
	}

	/**
	 * Render the widget output on the frontend.
	 *
	 * Written in PHP and used to generate the final HTML.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function render()
	{
		global $post;
		$current_slug = get_post_field('post_name', $post->ID);

		$is_admin = current_user_can('administrator');
		$kit_pages = $this->get_kit_pages($is_admin);
?>
		<div class="c8s-coach-kitpages-dropdown">
			<?php

			if (empty($kit_pages)) {
				echo '<div>No kit pages allowed.</div>';
			} else {
				echo '<select id="kit-page-dropdown" onchange="redirectToKitPage(this.value)">';
				foreach ($kit_pages as $kit_page) {
					$kit_slug = get_post_field('post_name', $kit_page->ID);
					echo '<option value="' . esc_url(get_permalink($kit_page->ID)) . '"';
					if ($kit_slug === $current_slug) {
						echo ' selected';
					}
					echo '>' . esc_html($kit_page->post_title);
					echo '</option>';
				}
				echo '</select>';
				echo '<script>
					function redirectToKitPage(url) {
						if (url) {
							window.location.href = url;
						}
					}
				</script>';
			}
			?>
		</div>
	<?php
	}

	/**
	 * Render the widget output in the editor.
	 *
	 * Written as a Backbone JavaScript template and used to generate the live preview.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function _content_template()
	{
		$kit_pages = $this->get_kit_pages(true);
		
		wp_register_style('crazy8s-elementor', plugins_url('/assets/css/crazy8s-elementor.css', ELEMENTOR_CRAZY8S_COACHES), array(), '1.0.2');
	?>
		<div class="c8s-coach-kitpages-dropdown">
			<?php
			if (empty($kit_pages)) {
				echo '<div>No kit pages allowed.</div>';
			} else {
				echo '<select id="kit-page-dropdown">';
				foreach ($kit_pages as $kit_page) {
					echo '<option>' . esc_html($kit_page->post_title) . '</option>';
				}
				echo '</select>';
			}
			?>
		</div>
<?php
	}

	/**
	 * Read the available kit pages.
	 * 
	 * @since 1.0.0
	 *
	 * @access private
	 */

	private function get_kit_pages($is_admin)
	{
		$user = wp_get_current_user();
		$roles = $user->roles;
		$kit_pages = [];

		$args = [
			'post_type' => 'kit-page',
			'posts_per_page' => -1,
			'meta_query' => [
				'relation' => 'AND',
				'year_clause' => [
					'key' => 'year',
					'compare' => 'EXISTS',
				],
				'season_clause' => [
					'key' => 'season',
					'compare' => 'EXISTS',
				],
			],
			'orderby' => [
				'year_clause' => 'DESC',
				'season_clause' => 'ASC',
			],
		];
		$query = new \WP_Query($args);

		while ($query->have_posts()) {
			$query->the_post();
			$kit_page_slug = get_post_field('post_name', get_the_ID());
			if ($is_admin || in_array($kit_page_slug, $roles)) {
				$kit_pages[] = get_post(get_the_ID());
			}
		}

		wp_reset_postdata();
		return $kit_pages;
	}
}
