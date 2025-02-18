<?php
/**
 * BedtimeMath Activity List class.
 *
 * @category   Class
 * @package    ElementorBedtimeMath
 * @subpackage WordPress
 * @author     Dug Steen <dug@bedtimemath.org>
 * @copyright  2023 Bedtime Math
 * @license    https://opensource.org/licenses/GPL-3.0 GPL-3.0-only
 * @link       link(https://www.benmarshall.me/build-custom-elementor-widgets/,
 *             Build Custom Elementor Widgets)
 * @since      1.0.0
 * php version 8.1
 */
namespace ElementorBedtimeMath\Widgets;
use Elementor\Widget_Base;
use Elementor\Controls_Manager;

// Security Note: Blocks direct access to the plugin PHP files.
defined( 'ABSPATH' ) || die();

/**
 * BedtimeMath widget class.
 *
 * @since 1.0.0
 */
class BedtimeMath_Activity_List extends Widget_Base {
	/**
	 * Class constructor.
	 *
	 * @param array $data Widget data.
	 * @param array $args Widget arguments.
	 */
	public function __construct( $data = array(), $args = null ) {
		parent::__construct( $data, $args );
		wp_register_style( 'bedtimemath-style', plugins_url( '/assets/css/bedtimemath.css', ELEMENTOR_BEDTIMEMATH ), array(), '1.0.0' );
		wp_register_style( 'bedtimemath-activity-list-style', plugins_url( '/assets/css/bedtimemath-activity-list.css', ELEMENTOR_BEDTIMEMATH ), array(), '1.0.0' );
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
	public function get_name() {
		return 'bedtimemath_activity';
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
	public function get_title() {
		return __( 'Bedtime Math Activity List', 'elementor-bedtimemath' );
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
	public function get_icon() {
		return 'eicon-youtube';
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
	public function get_categories() {
		return array( 'bedtimemath' );
	}
	
	/**
	 * Enqueue styles.
	 */
	public function get_style_depends() {
		return array( 'bedtimemath-style', 'bedtimemath-activity-list-style' );
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
	protected function _register_controls() {
		
		/* CONTENT ACTIVITY LIST */

		$this->start_controls_section(
			'content_activitylist_section',
			[
				'label' => __( 'Activity List', 'elementor-bedtimemath' ),
				'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
			]
		);

		$this->add_control(
			'activity_list',
			[
				'label' => esc_html__( 'Activities', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::REPEATER,
				'fields' => [
					[
						'name' => 'activity_title',
						'label' => esc_html__( 'Title', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::TEXT,
						'default' => 'Activity Title',
						'placeholder' => esc_html__( 'Activity title goes here', 'elementor-bedtimemath' ),
					],
					[
						'name' => 'activity_image',
						'label' => esc_html__( 'Image', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::MEDIA,
						'default' => [
							'url' => \Elementor\Utils::get_placeholder_image_src(),
						],
					],
					[
						'name' => 'activity_pdf_link',
						'label' => esc_html__( 'PDF Link', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::URL,
						'options' => [ 'url', 'is_external', 'nofollow' ],
						'default' => [
							'url' => '',
							'is_external' => true,
							'nofollow' => true,
							// 'custom_attributes' => '',
						],
						'label_block' => true,
					],
					[
						'name' => 'activity_time',
						'label' => esc_html__( 'Time', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::TEXT,
						'placeholder' => esc_html__( 'Activity time goes here', 'elementor-bedtimemath' ),
					],
					[
						'name' => 'activity_artprep',
						'label' => esc_html__( 'Art Prep', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::TEXT,
						'placeholder' => esc_html__( 'Art prep time goes here', 'elementor-bedtimemath' ),
					],
					[
						'name' => 'activity_mathplay',
						'label' => esc_html__( 'Math Play', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::TEXT,
						'placeholder' => esc_html__( 'Math play time goes here', 'elementor-bedtimemath' ),
					],
					[
						'name' => 'activity_numkids',
						'label' => esc_html__( '# of Kids', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::TEXT,
						'placeholder' => esc_html__( 'Number of kids goes here', 'elementor-bedtimemath' ),
					],
					[
						'name' => 'activity_math',
						'label' => esc_html__( 'What\'s the Math?', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::TEXT,
						'placeholder' => esc_html__( 'Math categories go here', 'elementor-bedtimemath' ),
					],
					[
						'name' => 'activity_include_video',
						'label' => esc_html__( 'Include Video', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::SWITCHER,
						'label_on' => esc_html__('Yes', 'elementor-bedtimemath'),
						'label_off' => esc_html__('No', 'elementor-bedtimemath'),
						'return_value' => 'yes',
						'default' => 'no'
					],
					[
						'name' => 'activity_video_text',
						'label' => esc_html__( 'Video Link Text', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::TEXT,
						'default' => esc_html__( 'Watch It Now!', 'elementor-bedtimemath' ),
					],
					[
						'name' => 'activity_video_link',
						'label' => esc_html__( 'Video Link', 'elementor-bedtimemath' ),
						'type' => \Elementor\Controls_Manager::URL,
						'options' => [ 'url', 'is_external', 'nofollow' ],
						'default' => [
							'url' => '',
							'is_external' => true,
							'nofollow' => true,
							// 'custom_attributes' => '',
						],
						'label_block' => true,
					],
				],
				'default' => [
					[
						'activity_title' => esc_html__( 'Activity #1', 'elementor-bedtimemath' ),
					],
					[
						'activity_title' => esc_html__( 'Activity #2', 'elementor-bedtimemath' ),
					],
				],
				'title_field' => '{{{ activity_title }}}',
			]
		);

		$this->end_controls_section();
		
		/* CONTENT SETTINGS */

		$this->start_controls_section(
		  'content_layout_section',
		  [
			'label' => __( 'Layout', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_group_control(
			\Elementor\Group_Control_Image_Size::get_type(),
			[
				// Usage: `{name}_size` and `{name}_custom_dimension`, in this case `activity_image_size` and `activity_image_custom_dimension`.
				'name' => 'activity_image', 
				'label' => 'Image Size',
				'include' => [],
				'exclude' => [],
				'default' => 'full',
			]
		);
		
		$this->add_control(
			'activity_image_margin',
			[
				'label' => esc_html__( 'Right Margin', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::SLIDER,
				'size_units' => [ 'px', '%', 'em', 'rem', 'custom' ],
				'range' => [
					'px' => [
						'min' => 0,
						'max' => 1000,
						'step' => 5,
					],
					'%' => [
						'min' => 0,
						'max' => 100,
					],
				],
				'default' => [
					'unit' => 'px',
					'size' => 6,
				],
				'selectors' => [
					'{{WRAPPER}} .activity-image' => 'margin-right: {{SIZE}}{{UNIT}};',
				],
			]
		);

		$this->end_controls_section();
		
		
		/* TITLE STYLE */

		$this->start_controls_section(
		  'style_title_section',
		  [
			'label' => __( 'Title', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_STYLE,
		  ]
		);

		$this->add_control(
			'title_text_color',
			[
				'label' => esc_html__( 'Text Color', 'textdomain' ),
				'type' => \Elementor\Controls_Manager::COLOR,
				'selectors' => [
					'{{WRAPPER}} .activity-title' => 'color: {{VALUE}}',
				],
			]
		);

		$this->add_group_control(
			\Elementor\Group_Control_Typography::get_type(),
			[
				'name' => 'title_typography',
				'selector' => '{{WRAPPER}} .activity-title',
			]
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
	protected function render() {
		
		// read the settings
		$settings = $this->get_settings_for_display();

		// go through the list
		if ( isset($settings['activity_list']) ) {

			echo '<div class="activity-list">';

			foreach ( $settings['activity_list'] as $activity ) {

				echo '<div class="activity">';

					if( isset($activity['activity_image']['id']) && !empty( $activity['activity_image']['id'] ) ) {
						if ( isset($settings['activity_image_size']) && !empty( $settings['activity_image_size'] ) ) {

							echo '<div class="activity-image">';
							
							if ('custom' === $settings['activity_image_size'] ) {
								echo wp_get_attachment_image($activity['activity_image']['id'], [
										$settings['activity_image_custom_dimension']['width'],
										$settings['activity_image_custom_dimension']['height']
									]);
							} else {
								echo wp_get_attachment_image($activity['activity_image']['id'], $settings['activity_image_size']);
							}
							
							echo '</div> <!-- activity-image -->'; // <div class="activity-image">
						}
					}

					echo '<div class="activity-details">';

						if( isset($activity['activity_title']) && !empty( $activity['activity_title'] ) ) {
							echo '<h3 class="activity-title">' .
								'<a href="' . $activity['activity_pdf_link']['url'] . '" target="_blank">' . 
								esc_html__($activity['activity_title']) . '</a></h3>';
						}

						if( isset($activity['activity_time']) && !empty( $activity['activity_time'] ) ) {
							echo '<div class="activity-time">' . 
								'<span class="activity-label">Time:</span>' . 
								esc_html__($activity['activity_time']) . '</div>';
						}

						if( isset($activity['activity_artprep']) && !empty( $activity['activity_artprep'] ) ) {
							echo '<div class="activity-artprep">' . 
								'<span class="activity-label">Art Prep:</span>' . 
								esc_html__($activity['activity_artprep']) . '</div>';
						}

						if( isset($activity['activity_mathplay']) && !empty( $activity['activity_mathplay'] ) ) {
							echo '<div class="activity-mathplay">' . 
								'<span class="activity-label">Math Play:</span>' . 
								esc_html__($activity['activity_mathplay']) . '</div>';
						}

						if( isset($activity['activity_numkids']) && !empty( $activity['activity_numkids'] ) ) {
							echo '<div class="activity-numkids">' . 
								'<span class="activity-label"># of Kids:</span>' . 
								esc_html__($activity['activity_numkids']) . '</div>';
						}

						if( isset($activity['activity_math']) && !empty( $activity['activity_math'] ) ) {
							echo '<div class="activity-math">' . 
								'<span class="activity-label">What\'s the Math?</span>' . 
								esc_html__($activity['activity_math']) . '</div>';
						}

						if( 'yes' === $activity['activity_include_video'] ) {
							echo '<div class="activity-video">' . 
								'<a href="' . $activity['activity_video_link']['url'] . ' target="_blank">' . 
								esc_html__($activity['activity_video_text']) . '</a>' .
								'</div>';
						}
				
					echo '</div> <!-- activity-details -->'; // <div class="activity-details">

				echo '</div> <!-- activity -->'; // <div class="activity">

			} // foreach ( $settings['activity_list'] as $activity )

			echo '</div> <!-- activity-list -->'; // <div class="activity-list">
		}

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
	protected function content_template() {
		/* Not needed. See: https://github.com/elementor/elementor/issues/9208 */
	}
}
?>
